using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlast : MonoBehaviour
{
    [HideInInspector] public int Score;
    [HideInInspector] public bool PlayerDead;

    [SerializeField] RageBlastPlatform[] platforms;
    [SerializeField] float timeToDisablePlatform;
    [SerializeField] GameObject[] layouts;

    bool trackTime = true;
    float currentTime = 0;
    int lastNumber = -1;
    int nonDuplicateRandom;

    private void Start()
    {
        nonDuplicateRandom = lastNumber;
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }

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
        while(lastNumber == nonDuplicateRandom)
        {
            lastNumber = UnityEngine.Random.Range(0, platforms.Length);
        }

        nonDuplicateRandom = lastNumber;
        platforms[nonDuplicateRandom].DisableMe();
    }
}
