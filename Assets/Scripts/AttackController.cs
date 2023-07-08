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

    public GameObject bulletPrefab;
    public LaserScript laserV;
    public LaserScript laserH;
    public Image timer;

    private float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        lastShotTime = Time.time + INITIAL_ATTACK_CD;
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

        float timerFraction = (Time.time - lastShotTime) / ATTACK_CD;
        timerFraction = Mathf.Clamp01(timerFraction);

        timer.transform.localScale = new Vector3((1 - timerFraction), 1, 1);
        timer.transform.position = new Vector3(401.5f + (1 - timerFraction) * 100f, timer.transform.position.y, 0);
    }
}
