using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    public float INITIAL_ATTACK_CD = 2f;
    public float ATTACK_CD = 1f;
    public float INDICATOR_DURATION = 0.3f;
    public float LASER_DURATION = 0.2f;
    public int MAX_HP = 100;
    public int laserMode = 0; // 0 = +, 1 = x
    public bool alternateShotDirection = false;

    public IndicatorScript indicatorV;
    public IndicatorScript indicatorH;
    public IndicatorScript indicatorDV;
    public IndicatorScript indicatorDH;

    private UIManager uiManager;
    private Simulation simulation;

    private float lastIndicatorTime;
    private float lastShotTime;
    private int hitPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        simulation = GameObject.FindObjectOfType<Simulation>();
        lastIndicatorTime = Time.time + INITIAL_ATTACK_CD;
        lastShotTime = Time.time + ATTACK_CD + INITIAL_ATTACK_CD;
        this.SetHitpoints(MAX_HP);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastIndicatorTime > ATTACK_CD)
        {
            lastIndicatorTime = Time.time;

            if(laserMode == 0)
            {
                indicatorV.gameObject.SetActive(true);
                indicatorH.gameObject.SetActive(true);
                indicatorV.SetTimer();
                indicatorH.SetTimer();

                if(alternateShotDirection)
                {
                    laserMode = 1;
                }
            }
            else
            {
                indicatorDV.gameObject.SetActive(true);
                indicatorDH.gameObject.SetActive(true);
                indicatorDV.SetTimer();
                indicatorDH.SetTimer();

                if (alternateShotDirection)
                {
                    laserMode = 0;
                }
            }
        }

        //when the indicator has fizzled, the shot starts so the time is updated
        if(Time.time - lastIndicatorTime > INDICATOR_DURATION)
        {
            lastShotTime = lastIndicatorTime + INDICATOR_DURATION;
        }

        uiManager.SetTimerBar(lastShotTime, ATTACK_CD);
    }

    public float GetLaserDuration()
    {
        return LASER_DURATION;
    }

    public float GetIndicatorDuration()
    {
        return INDICATOR_DURATION;
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
            simulation.NextScene();
        }
    }
}
