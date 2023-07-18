using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTheHammer : CombatMove
{
    [SerializeField] int startingScore;
    [SerializeField] float maxTime;

    float currentTime = 0;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        Score = startingScore;
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if((currentTime >= maxTime && !MoveEnded) || (PlayerDead && !MoveEnded))
        {
            EndMove();
        }
    }

    public void CheckScore()
    {
        if(PlayerDead)
        {
            EndMove();
        }

        if(Score <= 0 && !MoveEnded)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;
        if(PlayerDead)
        {
            Score = 0;
        }
        Debug.Log(Score);
    }
}
