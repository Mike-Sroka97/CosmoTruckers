using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private MusicTracks sceneTrack;
    [SerializeField] private float fadeDuration = 0.25f; 
    [SerializeField] private bool playOnLoad = true; 

    // Start is called before the first frame update
    void Start()
    {
        if (playOnLoad)
        {
            PlaySceneTrack(); 
        }
    }

    public void PlaySceneTrack()
    {
        AudioManager.Instance.PlayTrack(sceneTrack, fadeDuration); 
    }
}
