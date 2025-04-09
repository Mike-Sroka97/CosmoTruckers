using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Master 
    public AudioMixer MasterMixer;
    public float MasterVolume { get; private set; } = 1f;

    // Music
    [SerializeField] private List<Music> AllTracks = new List<Music>();
    public float MusicVolume { get; private set; } = 0.5f;
    public AudioSource CurrentMusic { get; private set; }
    public AudioSource AlternateMusic { get; private set; }
    private Dictionary<MusicTracks, AudioClip> tracks = new Dictionary<MusicTracks, AudioClip>();
    private MusicTracks CurrentTrack = MusicTracks.None;

    // SFX
    /// <summary>
    /// List of all Sfx Audio Sources
    /// </summary>
    public List<AudioSource> SfxSources = new List<AudioSource>();
    public float SfxVolume { get; private set; } = 1f;

    // Dialog
    /// <summary>
    /// List of all Dialog Audio Sources
    /// </summary>
    public List<AudioSource> DialogSources = new List<AudioSource>();
    public float DialogVolume { get; private set; } = 1f;

    //Set instance or remove object
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SetupMusic();

            SettingsData temporarySettingsData = SettingsManager.LoadSettingsData();
            UpdateVolumes(temporarySettingsData);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region SFX
    /// <summary>
    /// Add a new sfx audio source to the list for updating purposes
    /// </summary>
    /// <param name="source"></param>
    public void AddSfxSource(AudioSource source)
    {
        source.volume = SfxVolume * MasterVolume;
        source.outputAudioMixerGroup = MasterMixer.FindMatchingGroups("SFX")[0]; 
        SfxSources.Add(source);
    }

    /// <summary>
    /// Remove an sfx audio source from the list that has been destroyed to avoid future duplications
    /// </summary>
    public void RemoveSfxSource(AudioSource source)
    {
        SfxSources.Remove(source);
    }

    /// <summary>
    /// Update all listed sfx audio sources with the current volume
    /// NEED TO IMPLEMENT: Add test sound that plays after adjusting
    /// </summary>
    public void UpdateSfxVolumes()
    {
        foreach (AudioSource source in SfxSources)
        {
            source.volume = SfxVolume * MasterVolume;
        }
    }
    #endregion

    #region Music
    private void SetupMusic()
    {
        // Add all music to a dictionary
        foreach (Music track in AllTracks)
        {
            tracks.Add(track.Track, track.AudioClip);
        }

        // Create the audio source
        CurrentMusic = gameObject.AddComponent<AudioSource>();
        CurrentMusic.volume = MusicVolume * MasterVolume;
        CurrentMusic.loop = true;
        CurrentMusic.outputAudioMixerGroup = MasterMixer.FindMatchingGroups("Music")[0];

        // Create the alternate audio source (for overworld)
        AlternateMusic = gameObject.AddComponent<AudioSource>();
        AlternateMusic.volume = MusicVolume * MasterVolume;
        AlternateMusic.loop = true;
        CurrentMusic.outputAudioMixerGroup = MasterMixer.FindMatchingGroups("Music")[0];
    }

    /// <summary>
    /// Fade in a track
    /// </summary>
    /// <param name="track"></param>
    /// <param name="duration"></param>
    public void PlayTrack(MusicTracks track, float duration, float fadeBetweenDuration = 0f)
    {
        if (CurrentTrack != track)
        {
            if (tracks.TryGetValue(track, out AudioClip clip))
            {
                StartCoroutine(FadeInNewTrack(CurrentMusic, duration, clip, fadeBetweenDuration));
            }
        }
        else
        {
            Debug.Log($"Attempting to play {track}, but track is already playing!"); 
        }
    }
    public void StopTrack(float duration)
    {
        StartCoroutine(FadeTrack(CurrentMusic, duration, null));
    }

    /// <summary>
    /// Fades in alternate track
    /// </summary>
    /// <param name="track"></param>
    /// <param name="duration"></param>
    /// <param name="fadeBetweenDuration"></param>
    public void PlayAlternateTrack(MusicTracks track, float duration)
    {
        if (CurrentTrack != track)
        {
            if (tracks.TryGetValue(track, out AudioClip clip))
            {
                StartCoroutine(FadeInNewTrack(AlternateMusic, duration, clip));
            }
        }
        else
        {
            Debug.Log($"Attempting to play {track}, but track is already playing!");
        }
    }
    public void StopAlternateTrack(float duration)
    {
        StartCoroutine(FadeTrack(AlternateMusic, duration, null));
    }

    /// <summary>
    /// Fade out current track and fade in new one
    /// </summary>
    /// <param name="source"></param>
    /// <param name="duration"></param>
    /// <param name="newTrack"></param>
    /// <param name="fadeBetweenDuration"></param>
    /// <returns></returns>
    private IEnumerator FadeInNewTrack(AudioSource source, float duration, AudioClip newTrack, float fadeBetweenDuration = 0f)
    {
        if (source.isPlaying)
        {
            StartCoroutine(FadeTrack(source, duration, null, fadeIn: false));

            yield return new WaitForEndOfFrame();

            while (source.isPlaying)
                yield return null;

            yield return new WaitForSeconds(fadeBetweenDuration);
        }

        StartCoroutine(FadeTrack(source, duration, newTrack, fadeIn: true));
    }

    /// <summary>
    /// Fade out current song. <br></br>
    /// Fade in new song if one is passed in.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeTrack(AudioSource source, float duration, AudioClip newTrack, bool fadeIn = false)
    {
        float original = source.volume;

        // Fade Out
        if (!fadeIn)
        {
            while (source.volume > 0f)
            {
                source.volume -= original * Time.deltaTime / duration;
                yield return null;
            }

            source.Stop();
            source.volume = original;
        }

        // Fade In
        else
        {
            source.volume = 0f;
            source.clip = newTrack;
            source.Play();

            while (source.volume < original)
            {
                source.volume += original * Time.deltaTime / duration;
                yield return null;
            }

            source.volume = original;
        }
    }

    /// <summary>
    /// Update the tracks playing with the new modified volumes
    /// </summary>
    public void UpdateMusicVolumes()
    {
        CurrentMusic.volume = MusicVolume * MasterVolume;
        AlternateMusic.volume = MusicVolume * MasterVolume;
    }

    [Serializable]
    private class Music
    {
        // Class used for finding music
        public MusicTracks Track;
        public AudioClip AudioClip;
    }
    #endregion

    /// <summary>
    /// Update all audio volumes
    /// </summary>
    /// <param name="data"></param>
    public void UpdateVolumes(SettingsData data)
    {
        MasterVolume = data.MasterVolume / 100f;
        MusicVolume = data.MusicVolume / 100f;
        SfxVolume = data.SfxVolume / 100f;
        DialogVolume = data.DialogVolume / 100f;

        UpdateSfxVolumes();
        UpdateMusicVolumes(); 
    }
}

[Serializable]
public enum MusicTracks
{
    None,
    Menu_Title, 
    Menu_TitleFaded, 
    Tutorial,
    Hub_NowheresEnd, 
    Hub_Training, 
    D1_Overworld, 
    D1_Dungeon, 
    D1_Combat, 
    D1_Miniboss, 
    D1_Miniboss2,
    D1_Orbnus, 
    D1_Qmuav, 
    D9_Malice
}