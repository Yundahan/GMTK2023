using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject enemyPrefab;
    public GameObject areaPrefab;

    public TextMeshProUGUI killcount;
    public TextMeshProUGUI savedcount;

    public string nextLevel;

    private float enemyTimer;
    private float healAreaTimer;
    private int kills = 0;
    private int saves = 0;

    void Awake()
    {
        enemyTimer = Time.time + ENEMY_SPAWN_CD;
        healAreaTimer = Time.time + HEAL_AREA_CD;
    }

    void Update()
    {
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
    }

    public void UpdateCounts(bool saved)
    {
        if(saved)
        {
            saves++;
            savedcount.text = saves.ToString();
        }
        else
        {
            kills++;
            killcount.text = kills.ToString();
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

    public void NextScene()
    {
        StartCoroutine(LoadNextScene());
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
