using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Master Mixer")]
    [SerializeField] AudioMixer masterMixer;
    [Header("Audio sorces")] 
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [Header("Music clips")]
    //Can add overworld, combat and any other level music as needed
    [SerializeField] private AudioClip[] levelMusic;

    private int currentMusicIndex = -1;
    private float currentSongEndingTime;

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

    //Get default volume and current song length
    private void Start()
    {
        CheckVolume();
        CheckLevelSong();
        currentSongEndingTime = (float)AudioSettings.dspTime + musicAudioSource.clip.length;
    }

    //if song is over switch songs/reset
    private void Update()
    {
        if (AudioSettings.dspTime > currentSongEndingTime)
        {
            CheckLevelSong();
        }
    }

    public void CheckLevelSong()
    {
        currentMusicIndex++;

        if (currentMusicIndex >= levelMusic.Length)
        {
            //Reset index
            currentMusicIndex = 0;
        }

        musicAudioSource.clip = levelMusic[currentMusicIndex];
        musicAudioSource.Play();

        currentSongEndingTime = (float)AudioSettings.dspTime + musicAudioSource.clip.length;
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxAudioSource.PlayOneShot(sfx);
    }

    public void CheckVolume()
    {
        //Playerprefs or if we put the volume in custom save
        float sfxVolume = PlayerPrefs.GetFloat("SFX", .5f);
        float musicVolume = PlayerPrefs.GetFloat("Music", .5f);


        masterMixer.SetFloat("musicVol", musicVolume == -1 ? -80 : musicVolume * 20) ;
        masterMixer.SetFloat("sfxVol", sfxVolume == -1 ? -80 : sfxVolume * 20);
    }

    public void TestSFXVolume(float vol)
    {
        masterMixer.SetFloat("sfxVol", vol == -1 ? -80 : vol * 20);

        if (!sfxAudioSource.isPlaying)
            sfxAudioSource.Play();
    }
    public void TestMusicVolume(float vol)
    {
        masterMixer.SetFloat("musicVol", vol == -1 ? -80 : vol * 20);
    }
}
