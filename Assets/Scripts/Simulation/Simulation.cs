using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simulation : MonoBehaviour
{
    public float ENEMY_SPAWN_CD = 1f;
    public float HEAL_AREA_CD = 2f;
    public int HEAL_VALUE = 10;
    public int DAMAGE_VALUE = -5;
    public bool HEAL_AREAS_ACTIVE = false;

    private float PLAYING_FIELD_HEIGHT = 10f;
    private float PLAYING_FIELD_WIDTH = 18f;
    private float BOUNDS_VERTICAL = 4f;
    private float BOUNDS_HORIZONTAL = 8f;
    private float LEVEL_END_TIME = 1f;

    public GameObject enemyPrefab;
    public GameObject areaPrefab;

    public AudioSource audioSource;

    private UIManager uiManager;
    private AttackController attackController;
    private Slider slider;

    public string nextLevel;

    private float enemyTimer;
    private float healAreaTimer;
    private float levelFinishedTimer;
    private int kills = 0;
    private bool simulationRunning = true;
    private bool levelFinished = false;
    private GameState gameState;

    static float audioTimer;
    static float audioVolumeValue = 0.1F;
    static List<int> killCountList = new List<int>();

    private struct GameState
    {
        public float enemyTimerDifference;
        public float healAreaTimerDifference;
    }

    void Awake()
    {
        enemyTimer = Time.time + ENEMY_SPAWN_CD;
        healAreaTimer = Time.time + HEAL_AREA_CD;
        uiManager = GameObject.FindObjectOfType<UIManager>();
        attackController = GameObject.FindObjectOfType<AttackController>();
        slider = GameObject.FindObjectOfType<Slider>();
    }

    void Update()
    {
        if(levelFinished && Time.time - levelFinishedTimer > LEVEL_END_TIME)
        {
            NextScene();
        }

        bool started = false;

        if (Input.GetKeyDown(KeyCode.Space) && !IsSimulationRunning())
        {
            StartSimulation();
            started = true;
        }

        if (!IsSimulationRunning())
        {
            return;
        }

        if (Time.time - enemyTimer > ENEMY_SPAWN_CD)
        {
            enemyTimer = Time.time;

            GameObject enemy = Instantiate(enemyPrefab, GetEnemySpawnPosition(), Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().SetHealValue(HEAL_VALUE);
            enemy.GetComponent<EnemyMovement>().SetDamageValue(DAMAGE_VALUE);
        }

        if (HEAL_AREAS_ACTIVE && Time.time - healAreaTimer > HEAL_AREA_CD)
        {
            healAreaTimer = Time.time;

            Vector3 areaPosition = new Vector3(Random.Range(-BOUNDS_HORIZONTAL, BOUNDS_HORIZONTAL), Random.Range(-BOUNDS_VERTICAL, BOUNDS_VERTICAL), 0);
            GameObject healArea = Instantiate(areaPrefab, areaPosition, Quaternion.identity);
            healArea.GetComponent<HealAreaScript>().StartIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsSimulationRunning() &&!started)
        {
            PauseSimulation();
        }
    }

    public void UpdateCounts(bool saved)
    {
        if(!saved)
        {
            kills++;
            uiManager.SetKillCount(kills);
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        // check if we want to spawn on the short or long sides
        if (Random.Range(-1f, 1f) > 0f)
        {
            //check if we want to spawn left or right
            if (Random.Range(-1f, 1f) > 0f)
            {
                return new Vector3(-PLAYING_FIELD_WIDTH / 2, Random.Range(-PLAYING_FIELD_HEIGHT / 2, PLAYING_FIELD_HEIGHT / 2), 0);
            }
            else
            {
                return new Vector3(PLAYING_FIELD_WIDTH / 2, Random.Range(-PLAYING_FIELD_HEIGHT / 2, PLAYING_FIELD_HEIGHT / 2), 0);
            }
        }
        else
        {
            //check if we want to spawn up or down
            if (Random.Range(-1f, 1f) > 0f)
            {
                return new Vector3(Random.Range(-PLAYING_FIELD_WIDTH / 2, PLAYING_FIELD_WIDTH / 2), PLAYING_FIELD_HEIGHT / 2, 0);
            }
            else
            {
                return new Vector3(Random.Range(-PLAYING_FIELD_WIDTH / 2, PLAYING_FIELD_WIDTH / 2), -PLAYING_FIELD_HEIGHT / 2, 0);
            }
        }
    }
    public static void BroadcastAll(string methodName)
    {
        GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject gameObject in gameObjects)
        {
            EnemyMovement enemyMovement = gameObject.GetComponent<EnemyMovement>();
            HealAreaScript healArea = gameObject.GetComponent<HealAreaScript>();
            IndicatorScript indicator = gameObject.GetComponent<IndicatorScript>();
            LaserScript laser = gameObject.GetComponent<LaserScript>();

            if (enemyMovement != null)
            {
                enemyMovement.SendMessage(methodName);
            }
            if (healArea != null)
            {
                healArea.SendMessage(methodName);
            }
            if (indicator != null)
            {
                indicator.SendMessage(methodName);
            }
            if (laser != null)
            {
                laser.SendMessage(methodName);
            }
        }
    }

    public bool IsSimulationRunning()
    {
        return simulationRunning;
    }

    public void StartSimulation()
    {
        if(levelFinished)
        {
            return;
        }

        simulationRunning = true;
        uiManager.HidePauseImage();
        enemyTimer = Time.time - gameState.enemyTimerDifference;
        healAreaTimer = Time.time - gameState.healAreaTimerDifference;
        attackController.StartSimulationGO();
        BroadcastAll("StartSimulationGO");
    }

    public void PauseSimulation()
    {
        simulationRunning = false;
        uiManager.ShowPauseImage();
        gameState.enemyTimerDifference = Time.time - enemyTimer;
        gameState.healAreaTimerDifference = Time.time - healAreaTimer;
        attackController.PauseSimulationGO();
        BroadcastAll("PauseSimulationGO");
    }

    public void EndLevel()
    {
        levelFinished = true;
        levelFinishedTimer = Time.time;
        simulationRunning = false;
    }

    public void NextScene()
    {
        audioVolumeValue = slider.value;
        audioTimer = audioSource.time;
        killCountList.Add(kills);

        StartCoroutine(LoadNextScene());
    }

    public float GetAudioTimer()
    {
        return audioTimer;
    }

    public float GetAudioVolumeValue()
    {
        return audioVolumeValue;
    }

    public List<int> GetKillCountList()
    {
        return killCountList;
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
