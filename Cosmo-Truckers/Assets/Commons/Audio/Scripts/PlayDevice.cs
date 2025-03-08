using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays all audio on audio device upon loading in
/// </summary>
public class PlayDevice : MonoBehaviour
{
    private AudioDevice myAudioDevice;

    void Start()
    {
        myAudioDevice = GetComponent<AudioDevice>();

        foreach (var audio in myAudioDevice.MyAudio)
        {
            myAudioDevice.PlaySound(audio.Name);
        }
    }
}
