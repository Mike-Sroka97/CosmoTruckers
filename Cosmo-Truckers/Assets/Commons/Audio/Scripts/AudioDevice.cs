using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEditor.Progress;

public class AudioDevice : MonoBehaviour
{
    [Serializable]
    public class Audio
    {
        // Class used for adding audio to this device
        public string Name;
        public AudioClip AudioClip;
        public bool Loop;
    }

    /// <summary>
    /// Audio to create sources for on this device
    /// </summary>
    [SerializeField] List<Audio> myAudio = new List<Audio>();
    
    /// <summary>
    /// Public getter for the audio on this device
    /// </summary>
    public List<Audio> MyAudio
    {
        get { return myAudio; }
    }

    /// <summary>
    /// The sources on this device
    /// </summary>
    private Dictionary<string, AudioSource> myAudioSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        SetupAudio(); 
    }

    private void SetupAudio()
    {
        foreach (Audio audio in myAudio)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = audio.AudioClip;
            newSource.loop = audio.Loop;
            myAudioSources.Add(audio.Name, newSource); 

            AudioManager.Instance.AddSfxSource(newSource);
        }
    }

    /// <summary>
    /// Play the sound passed in
    /// </summary>
    /// <param name="name"></param>
    public AudioSource PlaySound(string name)
    {
        AudioSource source = GetSound(name);
        if (source != null) { source.Play(); }

        return source;
    }

    /// <summary>
    /// Plays a random sound out of a passed-in array of names
    /// </summary>
    public AudioSource PlayRandomSound(string[] names)
    {
        int random = UnityEngine.Random.Range(0, names.Length);
        return PlaySound(names[random]);
    }

    /// <summary>
    /// Stop the sound passed in
    /// </summary>
    /// <param name="name"></param>
    public AudioSource StopSound(string name)
    {
        AudioSource source = GetSound(name);
        if (source != null) { source.Stop(); }

        return source;
    }

    /// <summary>
    /// Stops all other sounds on this device and plays the specified sound
    /// </summary>
    /// <param name="name"></param>
    public AudioSource StopAllAndPlay(string name)
    {
        AudioSource sourceToPlay = null;

        foreach (KeyValuePair<string, AudioSource> item in myAudioSources)
        {
            if (item.Key == name)
            {
                sourceToPlay = item.Value;
            }
            else
            {
                item.Value.Stop();
            }
        }

        if (sourceToPlay != null)
        {
            sourceToPlay.Play();
            return sourceToPlay; 
        }

        return null; 
    }

    /// <summary>
    /// Get a sound from the device
    /// </summary>
    /// <param name="name"></param>
    public AudioSource GetSound(string name)
    {
        if (myAudioSources.TryGetValue(name, out AudioSource source))
        {
            return source;
        }
        else
        {
            Debug.LogError("There is no audio on this device with that name!");
            return null;
        }
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<string, AudioSource> item in myAudioSources)
        {
            AudioManager.Instance.RemoveSfxSource(item.Value);
        }
    }
}

/// <summary>
/// General minigame sounds
/// </summary>
public enum SfxMinigame
{
    Countdown,
    EnemyHit,
    EnemyDie, 
    ItemBad, 
    ItemGood, 
}

/// <summary>
/// General player minigame sounds
/// </summary>
public enum SfxPlayerMg
{
    Jump, 
    Attack, 
    Special, 
    Hurt, 
    Death, 
}
