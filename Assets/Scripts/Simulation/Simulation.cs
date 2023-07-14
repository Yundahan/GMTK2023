using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simulation : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject areaPrefab;

    public AudioSource bgmAudioSource;

    private UIManager uiManager;
    private AttackController attackController;
    private Slider slider;

    private GeneralConfig generalConfig;
    private LevelConfig levelConfig;
    public string levelConfigFileName;

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
        ConfigLoader configLoader = new ConfigLoader();
        levelConfig = configLoader.LoadLevelConfig(levelConfigFileName);
        generalConfig = configLoader.LoadGeneralConfig();

        uiManager = GameObject.FindObjectOfType<UIManager>();
        attackController = GameObject.FindObjectOfType<AttackController>();
        slider = GameObject.FindObjectOfType<Slider>();
        enemyTimer = Time.time + levelConfig.enemySpawnCD;
        healAreaTimer = Time.time + levelConfig.healAreaCD;
    }

    void Update()
    {
        if(levelFinished && Time.time - levelFinishedTimer > generalConfig.levelEndTime)
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

        if (Time.time - enemyTimer > levelConfig.enemySpawnCD)
        {
            enemyTimer = Time.time;

            GameObject enemy = Instantiate(enemyPrefab, GetEnemySpawnPosition(), Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().SetHealValue(levelConfig.healValue);
            enemy.GetComponent<EnemyMovement>().SetDamageValue(levelConfig.damageValue);
        }

        if (levelConfig.healAreasActive && Time.time - healAreaTimer > levelConfig.healAreaCD)
        {
            healAreaTimer = Time.time;

            Vector3 areaPosition = new Vector3(Random.Range(-generalConfig.healAreaBoundsHorizontal, generalConfig.healAreaBoundsHorizontal),
                Random.Range(-generalConfig.healAreaBoundsVertical, generalConfig.healAreaBoundsVertical), 0);
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
        float playingFieldWidth = generalConfig.playingFieldWidth;
        float playingFieldHeight = generalConfig.playingFieldHeight;

        // check if we want to spawn on the short or long sides
        if (Random.Range(-1f, 1f) > 0f)
        {
            //check if we want to spawn left or right
            if (Random.Range(-1f, 1f) > 0f)
            {
                return new Vector3(-playingFieldWidth / 2, Random.Range(-playingFieldHeight / 2, playingFieldHeight / 2), 0);
            }
            else
            {
                return new Vector3(playingFieldWidth / 2, Random.Range(-playingFieldHeight / 2, playingFieldHeight / 2), 0);
            }
        }
        else
        {
            //check if we want to spawn up or down
            if (Random.Range(-1f, 1f) > 0f)
            {
                return new Vector3(Random.Range(-playingFieldWidth / 2, playingFieldWidth / 2), playingFieldHeight / 2, 0);
            }
            else
            {
                return new Vector3(Random.Range(-playingFieldWidth / 2, playingFieldWidth / 2), -playingFieldHeight / 2, 0);
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
        audioTimer = bgmAudioSource.time;
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

    //this should be used only in awake or start functions, to be safe if the simulation awakes later than the script in question
    public GeneralConfig ReloadGeneralConfig()
    {
        ConfigLoader configLoader = new ConfigLoader();
        generalConfig = configLoader.LoadGeneralConfig();
        return generalConfig;
    }

    //this should not be used in awake or start functions - use ReloadGeneralConfig instead
    public GeneralConfig GetGeneralConfig()
    {
        return generalConfig;
    }

    public LevelConfig GetLevelConfig()
    {
        return levelConfig;
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelConfig.nextLevelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
