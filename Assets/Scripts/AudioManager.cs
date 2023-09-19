using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("AudioClip")]
    public AudioClip background;
    public AudioClip moveCandy;
    public AudioClip Lose;
    public AudioClip Win;
    public AudioClip Ketlieu;
    public AudioClip RecoverHp;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip audio)
    {
        sfxSource.PlayOneShot(audio);
    }
}
