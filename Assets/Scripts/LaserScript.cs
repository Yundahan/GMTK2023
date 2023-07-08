using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private AttackController attackController;

    private float timer;
    public AudioSource LaserAudio;
    private bool LaserActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        attackController = this.transform.parent.GetComponent<AttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserActive)
        {
            LaserAudio.Play();
            LaserActive = false;
        }
        if(Time.time - timer > attackController.GetLaserDuration())
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
