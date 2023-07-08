using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public AudioSource LaserAudio;

    private AttackController attackController;

    private float timer;

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
        if(Time.time - timer > attackController.GetLaserDuration())
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTimer()
    {
        timer = Time.time;
        LaserAudio.Play();
    }
}
