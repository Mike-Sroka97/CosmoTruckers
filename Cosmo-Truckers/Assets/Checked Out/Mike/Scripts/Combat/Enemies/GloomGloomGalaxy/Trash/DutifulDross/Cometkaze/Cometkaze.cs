using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cometkaze : CombatMove
{
    [SerializeField] float maxSuccessTime;

    float currentTime = 0;

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackDeath()
    {
        if(PlayerDead)
        {
            EndMove();
        }
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= maxSuccessTime)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        if (MoveEnded)
            return;

        MoveEnded = true;

        if(currentTime > maxSuccessTime)
        {
            currentTime = maxSuccessTime;
        }

        Score = (int)currentTime;
    }
}
