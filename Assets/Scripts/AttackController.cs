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
    public float LASER_DURATION = 0.2f;
    public int MAX_HP = 100;

    public GameObject bulletPrefab;
    public LaserScript laserV;
    public LaserScript laserH;
    public UIManager uiManager;

    private float lastShotTime;
    private int hitPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        lastShotTime = Time.time + INITIAL_ATTACK_CD;
        hitPoints = MAX_HP;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastShotTime > ATTACK_CD)
        {
            lastShotTime = Time.time;

            laserH.gameObject.SetActive(true);
            laserV.gameObject.SetActive(true);
            laserH.SetTimer();
            laserV.SetTimer();
        }

        uiManager.SetTimerBar(lastShotTime, ATTACK_CD);
    }

    public float GetLaserDuration()
    {
        return LASER_DURATION;
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
    }
}
