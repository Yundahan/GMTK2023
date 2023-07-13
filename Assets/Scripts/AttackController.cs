using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float INITIAL_ATTACK_CD = 2f;
    public int MAX_HP = 100;

    public IndicatorScript indicatorV;
    public IndicatorScript indicatorH;
    public IndicatorScript indicatorDV;
    public IndicatorScript indicatorDH;

    private UIManager uiManager;
    private Simulation simulation;

    private float lastIndicatorTime;
    private float lastShotTime;
    private int hitPoints;
    private GameState gameState;

    private struct GameState
    {
        public float lastIndicatorTimeDifference;
        public float lastShotTimeDifference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        simulation = GameObject.FindObjectOfType<Simulation>();
        lastIndicatorTime = Time.time + INITIAL_ATTACK_CD;
        lastShotTime = Time.time + INITIAL_ATTACK_CD;
        this.SetHitpoints(MAX_HP);
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if(Time.time - lastIndicatorTime > simulation.GetLevelConfig().playerAttackCD)
        {
            lastIndicatorTime = Time.time;

            if(simulation.GetLevelConfig().laserMode == 0)
            {
                indicatorV.gameObject.SetActive(true);
                indicatorH.gameObject.SetActive(true);
                indicatorV.SetTimer();
                indicatorH.SetTimer();

                if(simulation.GetLevelConfig().alternateShotDirection)
                {
                    simulation.GetLevelConfig().laserMode = 1;
                }
            }
            else
            {
                indicatorDV.gameObject.SetActive(true);
                indicatorDH.gameObject.SetActive(true);
                indicatorDV.SetTimer();
                indicatorDH.SetTimer();

                if (simulation.GetLevelConfig().alternateShotDirection)
                {
                    simulation.GetLevelConfig().laserMode = 0;
                }
            }
        }

        //when the indicator has fizzled, the shot starts so the time is updated
        if(Time.time - lastIndicatorTime > simulation.GetGeneralConfig().laserIndicatorDuration)
        {
            lastShotTime = lastIndicatorTime + simulation.GetGeneralConfig().laserIndicatorDuration;
        }
    }

    public int GetHitpoints()
    {
        return hitPoints;
    }

    public void SetHitpoints(int value)
    {
        hitPoints = value;
        uiManager.SetHPBar(hitPoints, MAX_HP);
    }

    public void ChangeHitpoints(int value)
    {
        hitPoints += value;
        hitPoints = Math.Clamp(hitPoints, 0, MAX_HP);
        uiManager.SetHPBar(hitPoints, MAX_HP);

        if(hitPoints == 0)
        {
            simulation.EndLevel();
        }
    }

    public void StartSimulationGO()
    {
        lastIndicatorTime = Time.time - gameState.lastIndicatorTimeDifference;
        lastShotTime = Time.time - gameState.lastShotTimeDifference;
    }

    public void PauseSimulationGO()
    {
        gameState.lastIndicatorTimeDifference = Time.time - lastIndicatorTime;
        gameState.lastShotTimeDifference = Time.time - lastShotTime;
    }
}
