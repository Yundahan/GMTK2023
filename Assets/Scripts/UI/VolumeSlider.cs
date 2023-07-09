using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioSource AudioS;
    [SerializeField]
    private AudioMixMode MixMode;

    private Simulation simulation;


    public void OnChangeSlider (float value)
    {
        SetVolume(value);
    }
    public enum AudioMixMode
    {

        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogarithmicMixerVolume
    }
    // Start is called before the first frame update
    void Start()
    {
        simulation = GameObject.FindObjectOfType<Simulation>();
        Slider slider = GetComponent<Slider>();
        slider.value = simulation.GetAudioVolumeValue();
        //SetVolume(simulation.GetAudioVolumeValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetVolume(float value)
    {
        switch (MixMode)
        {
            /*case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = value;
                break;*/
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + value * 80));
                break;
            case AudioMixMode.LogarithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(value) * 20);
                break;

        }
    }
}
