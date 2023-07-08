using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float LASER_DURATION = 0.2f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timer > LASER_DURATION)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTimer()
    {
        timer = Time.time;
    }
}
