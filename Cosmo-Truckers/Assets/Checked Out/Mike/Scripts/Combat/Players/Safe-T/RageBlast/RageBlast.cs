using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlast : CombatMove
{
    [SerializeField] RageBlastPlatform[] platforms;
    [SerializeField] float timeToDisablePlatform;

    bool trackTime = true;
    float currentTime = 0;
    int lastNumber = -1;
    int nonDuplicateRandom;

    private void Start()
    {
        StartMove();
        nonDuplicateRandom = lastNumber;
        GenerateLayout();
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

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
