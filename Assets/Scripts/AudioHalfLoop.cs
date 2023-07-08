using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHalfLoop : MonoBehaviour
{

    public AudioClip songStart;

    public AudioSource songLoop;

    public bool introToggle = true;

    public float rndSongTime;

    void Start()
    {
        /*songStart.Play();
        songLoop.Play();
        songLoop.Pause();
        */
    
     
        songLoop.PlayOneShot(songStart);
        songLoop.PlayScheduled(AudioSettings.dspTime + songStart.length);
        

    }

    /*void Update()
     {
        rndSongTime = Mathf.Round(songStart.time * 100f) / 100f;
        Debug.Log(songStart.time);
        if (rndSongTime == 9.60 && introToggle )
        {
            
            songLoop.UnPause();
            introToggle = false;

        }

     }*/

}
