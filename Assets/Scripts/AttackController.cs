using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float INITIAL_ATTACK_CD = 2f;
    public float ATTACK_CD = 1f;
    public float INDICATOR_DURATION = 0.3f;
    public float LASER_DURATION = 0.2f;
    public float SPEAR_DURATION = 0.4f;
    public int MAX_HP = 100;
    public int laserMode = 0; // 0 = +, 1 = x
    public bool alternateShotDirection = false;
    public ShotMode shotMode = ShotMode.Spear;

    public IndicatorScript indicatorV;
    public IndicatorScript indicatorH;
    public IndicatorScript indicatorDV;
    public IndicatorScript indicatorDH;

    public IndicatorScript indicatorRight;

    public GameObject projectilePrefab;

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

    public enum ShotMode
    {
        Laser,
        Spear,
        Projectile
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
        lastShotTime = Time.time + ATTACK_CD + INITIAL_ATTACK_CD;
        this.SetHitpoints(MAX_HP);
    }

    // Update is called once per frame
    void Update()
    {
        if (!simulation.IsSimulationRunning())
        {
            return;
        }

        if(shotMode == ShotMode.Laser)
        {
            HandleLaserMode();
        } 
        else if(shotMode == ShotMode.Spear)
        {
            HandleSpearMode();
        }
        else if(shotMode == ShotMode.Projectile)
        {
            HandleProjectileMode();
        }
    }

    private void HandleLaserMode()
    {
        if (Time.time - lastIndicatorTime > ATTACK_CD)
        {
            lastIndicatorTime = Time.time;

            if (laserMode == 0)
            {
                indicatorV.gameObject.SetActive(true);
                indicatorH.gameObject.SetActive(true);
                indicatorV.SetTimer();
                indicatorH.SetTimer();

                if (alternateShotDirection)
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
        if (Time.time - lastIndicatorTime > INDICATOR_DURATION)
        {
            lastShotTime = lastIndicatorTime + INDICATOR_DURATION;
        }
    }

    private void HandleSpearMode()
    {
        if (Time.time - lastIndicatorTime > ATTACK_CD)
        {
            lastIndicatorTime = Time.time;
            indicatorRight.gameObject.SetActive(true);
            indicatorRight.SetTimer();
        }

        //when the indicator has fizzled, the shot starts so the time is updated
        if (Time.time - lastIndicatorTime > INDICATOR_DURATION)
        {
            lastShotTime = lastIndicatorTime + INDICATOR_DURATION;
        }
    }

    private void HandleProjectileMode()
    {
        if (Time.time - lastShotTime > ATTACK_CD)
        {
            lastShotTime = Time.time;

            GameObject rightBullet = Instantiate(projectilePrefab, this.transform.position + Vector3.right, Quaternion.identity);
            rightBullet.GetComponent<ProjectileScript>().SetDirection(Vector3.right);
            GameObject downBullet = Instantiate(projectilePrefab, this.transform.position + Vector3.down, Quaternion.Euler(Vector3.forward * 90));
            downBullet.GetComponent<ProjectileScript>().SetDirection(Vector3.down);
            GameObject leftBullet = Instantiate(projectilePrefab, this.transform.position + Vector3.left, Quaternion.Euler(Vector3.forward * 180));
            leftBullet.GetComponent<ProjectileScript>().SetDirection(Vector3.left);
            GameObject upBullet = Instantiate(projectilePrefab, this.transform.position + Vector3.up, Quaternion.Euler(Vector3.forward * 270));
            upBullet.GetComponent<ProjectileScript>().SetDirection(Vector3.up);
        }
    }

    public float GetLaserDuration()
    {
        return LASER_DURATION;
    }

    public float GetSpearDuration()
    {
        return SPEAR_DURATION;
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
            simulation.EndLevel();
        }
    }

    public void SetShotMode(ShotMode shotMode)
    {
        this.shotMode = shotMode;
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
