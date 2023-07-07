using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    private float ENEMY_SPAWN_CD = 1f;
    private float PLAYING_FIELD_HEIGHT = 10f;
    private float PLAYING_FIELD_WIDTH = 18f;

    public GameObject enemyPrefab;

    public TextMeshProUGUI killcount;
    public TextMeshProUGUI savedcount;

    private float timer;
    private int kills = 0;
    private int saves = 0;

    void Awake()
    {
        timer = Time.time + ENEMY_SPAWN_CD;
    }

    void Update()
    {
        if(Time.time - timer > ENEMY_SPAWN_CD)
        {
            timer = Time.time;

            Instantiate(enemyPrefab, GetEnemySpawnPosition(), Quaternion.identity);
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
}
