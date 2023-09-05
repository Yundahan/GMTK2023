using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : AttackScript
{
    public AudioSource LaserAudio;

    private float timer;
    private GameState gameState;

    private struct GameState
    {
        public float timerDifference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if (Time.time - timer > attackController.GetLaserDuration())
        {
            gameObject.SetActive(false);
        }
    }

    public override void SetTimer()
    {
        timer = Time.time;
        LaserAudio.Play();
    }

    public override void StartSimulationGO()
    {
        timer = Time.time - gameState.timerDifference;
    }

    public override void PauseSimulationGO()
    {
        gameState.timerDifference = Time.time - timer;
    }
}
