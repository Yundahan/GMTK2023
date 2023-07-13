using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public AudioSource LaserAudio;

    private Simulation simulation;
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

    private void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if (Time.time - timer > simulation.GetGeneralConfig().laserDuration)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTimer()
    {
        timer = Time.time;
        LaserAudio.Play();
    }

    public void StartSimulationGO()
    {
        timer = Time.time - gameState.timerDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.timerDifference = Time.time - timer;
    }
}
