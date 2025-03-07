using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    Music,
    SFX,
    UI
}

// Singleton to manage the game audio.

public class AudioManager : MonoBehaviour
{
    [Header("------- Audio Source -------")]
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource UISource;

    [Header("------- Audio Clips -------")]
    public AudioClip backgroundMusic;


    private static AudioManager instance;


    private void Awake()
    {
        //Keep single instance of AudioManager between scenes.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public bool ToggleAudioType(AudioType type)
    {
        bool newState = !IsAudioTypeEnabled(type);
        ToggleAudioType(type, newState);
        return newState;
    }

    public void ToggleAudioType(AudioType soundType, bool toggle)
    {
        switch (soundType)
        {
            case AudioType.Music:
                musicSource.mute = !toggle;
                break;
            case AudioType.SFX:
                SFXSource.mute = !toggle;
                break;
            case AudioType.UI:
                UISource.mute = !toggle;
                break;
        }
    }

    public bool IsAudioTypeEnabled(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.Music:
                return !musicSource.mute;
            case AudioType.SFX:
                return !SFXSource.mute;
            case AudioType.UI:
                return !UISource.mute;
            default: return false;
        }
    }


    // implement your Awake, Start, Update, or other methods here... (optional)
}