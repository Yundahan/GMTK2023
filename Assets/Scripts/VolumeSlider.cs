using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioSource AudioS;
    [SerializeField]
    private TextMeshProUGUI ValueText;
    [SerializeField]
    private AudioMixMode MixMode;


    public void OnChangeSlider (float Value)
    {

        ValueText.SetText($"{Value.ToString("N4")}");

        switch(MixMode)
        {
            /*case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = Value;
                break;*/
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 80));
                break;
            case AudioMixMode.LogarithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;

        }
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
