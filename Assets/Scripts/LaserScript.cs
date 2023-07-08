using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float LASER_DURATION = 0.2f;

    private float timer;
    public AudioSource LaserAudio;
    private bool LaserActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserActive)
        {
            LaserAudio.Play();
            LaserActive = false;
        }
        if(Time.time - timer > LASER_DURATION)
        {
            gameObject.SetActive(false);
            LaserActive = true;
        }
    }

    public void SetTimer()
    {
        timer = Time.time;
    }
}
