using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    
    public void SetMusicVolume(float volume)
    {
        
        setVolume("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        setVolume("SFXVolume", volume);
    }

    private void setVolume(string name, float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(name, newVolume);
    }
}
