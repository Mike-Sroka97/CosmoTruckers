using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AudioDevice : MonoBehaviour
{
    [Serializable]
    /// <summary>
    /// Class used for adding audio to this device
    /// </summary>
    private class Audio
    {
        public string Name;
        public AudioClip AudioClip;
        public bool Loop;
    }

    /// <summary>
    /// Audio to create sources for on this device
    /// </summary>
    [SerializeField] List<Audio> myAudio = new List<Audio>();
    
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
    public void PlaySound(string name)
    {
        if (myAudioSources.TryGetValue(name, out AudioSource source))
        {
            source.Play(); 
        }
        else
        {
            Debug.LogError("There is no audio on this device with that name!"); 
            return; 
        }
    }

    /// <summary>
    /// Stop the sound passed in
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        if (myAudioSources.TryGetValue(name, out AudioSource source))
        {
            source.Stop();
        }
        else
        {
            Debug.LogError("There is no audio on this device with that name!");
            return;
        }
    }

    /// <summary>
    /// Stops all other sounds on this device and plays the specified sound
    /// </summary>
    /// <param name="name"></param>
    public void StopAllAndPlay(string name)
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

        if (sourceToPlay == null)
        {
            Debug.LogError("There is no audio on this device with that name!");
        }
        else
        {
            sourceToPlay.Play(); 
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
