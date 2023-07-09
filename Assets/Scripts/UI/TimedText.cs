using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedText : MonoBehaviour
{
    private float timer;
    private float duration = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        SpawnTextForXSeconds("intro text", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timer > duration)
        {
            gameObject.SetActive(false);
        }
    }

    public void SpawnTextForXSeconds(string text, float seconds)
    {
        timer = Time.time;
        duration = seconds;
        GetComponent<TextMeshProUGUI>().text = text;
        gameObject.SetActive(true);
    }
}
