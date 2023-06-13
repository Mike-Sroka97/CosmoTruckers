using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlast : MonoBehaviour
{
    [HideInInspector] public int Score;
    [HideInInspector] public bool PlayerDead;

    [SerializeField] RageBlastPlatform[] platforms;
    [SerializeField] float timeToDisablePlatform;

    bool trackTime = true;
    float currentTime = 0;
    int lastNumber = -1;

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if(trackTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeToDisablePlatform)
            {
                trackTime = false;
                NextPlatform();
            }
        }
    }

    public void NextPlatform()
    {
        int random = lastNumber;
        while(lastNumber == random)
        {
            random = UnityEngine.Random.Range(0, platforms.Length);
        }

        platforms[random].DisableMe();
    }
}
