using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public LaserScript laser;

    private AttackController attackController;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        attackController = GameObject.FindObjectOfType<AttackController>();
        timer = Time.time + 15f;
    }

    // Update is called once per frame
    void Update()
    {
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
}
