using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else { Destroy(gameObject); }
    }

    public void PlayAudio(string Audioname, AudioClip clip)
    {
        GameObject SoundObj = new GameObject(Audioname + "Sound");
        AudioSource audioSource = SoundObj.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(SoundObj, clip.length);
    }

    public void PlayAudio(string Audioname, AudioClip clip, float delaytime)
    {
        GameObject SoundObj = new GameObject(Audioname + "Sound");
        AudioSource audioSource = SoundObj.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.PlayDelayed(delaytime);;

        Destroy(SoundObj, clip.length);
    }

}
