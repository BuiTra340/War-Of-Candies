using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void LoadVolume()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
    public void SetMusicVolume()
    {
        float volume = MusicSlider.value;
        musicSource.volume = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        sfxSource.volume = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
}
