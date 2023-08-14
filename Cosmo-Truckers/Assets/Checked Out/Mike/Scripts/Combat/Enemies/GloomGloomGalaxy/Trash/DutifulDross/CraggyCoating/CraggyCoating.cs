using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoating : CombatMove
{
    [SerializeField] float maxTimeSuccess;

    bool trackTime = true;

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackDeath();
        TrackTime();
    }

    private void TrackDeath()
    {
        if (PlayerDead) //or time is up
        {
            EndMove();
        }
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= maxTimeSuccess)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        trackTime = false;
        if(currentTime > maxTimeSuccess)
        {
            currentTime = maxTimeSuccess;
        }
        Score = (int)(currentTime);
        Debug.Log(Score);
    }
}
