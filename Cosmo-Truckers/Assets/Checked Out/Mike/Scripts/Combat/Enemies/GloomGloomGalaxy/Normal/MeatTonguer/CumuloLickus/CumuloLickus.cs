using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloLickus : CombatMove
{
    [SerializeField] float maxTime = 20;

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

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= maxTime)
        {
            currentTime = maxTime;
            MoveEnded = true;
            EndMove();
        }
        else if(PlayerDead)
        {
            MoveEnded = true;
            EndMove();
        }
    }
    public override void EndMove()
    {
        Score = (int)currentTime;
    }
}
