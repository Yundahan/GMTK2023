using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public LaserScript laser;

    private AttackController attackController;

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

    void Awake()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        attackController = GameObject.FindObjectOfType<AttackController>();
        timer = Time.time + 15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if (Time.time - timer > attackController.GetIndicatorDuration())
        {
            laser.gameObject.SetActive(true);
            laser.SetTimer();
            gameObject.SetActive(false);
        }
    }

    public void SetTimer()
    {
        timer = Time.time;
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
