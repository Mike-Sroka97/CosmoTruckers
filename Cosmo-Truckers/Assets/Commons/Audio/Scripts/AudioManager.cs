using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    /// <summary>
    /// Master Audio Mixer
    /// </summary>
    public AudioMixer MasterMixer;

    /// <summary>
    /// List of all Sfx Audio Sources
    /// </summary>
    public List<AudioSource> SfxSources = new List<AudioSource>();

    /// <summary>
    /// List of all Music Audio Sources
    /// </summary>
    public List<AudioSource> MusicSources = new List<AudioSource>();

    /// <summary>
    /// The current SFX volume
    /// </summary>
    public float SfxCurrentVolume { get; private set; } = 1f;

    /// <summary>
    /// The current SFX volume
    /// </summary>
    public float MusicCurrentVolume { get; private set; } = 1f; 

    //Set instance or remove object
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddSfxSource(AudioSource source)
    {
        source.volume = SfxCurrentVolume;
        SfxSources.Add(source);
    }

    public void RemoveSfxSource(AudioSource source)
    {
        SfxSources.Remove(source);
    }

    public void UpdateSfxVolumes()
    {
        foreach (AudioSource source in SfxSources)
        {
            source.volume = SfxCurrentVolume;
        }
    }
}
