using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBall : CombatMove
{
    [SerializeField] float maxTime;

    float currentTime = 0;

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        if(!MoveEnded)
        {
            TrackTime();
        }        
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= maxTime)
        {
            currentTime = maxTime;
            EndMove();
        }
        else if(PlayerDead)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        Score = (int)currentTime;
        Debug.Log(Score);
        MoveEnded = true;
    }
}
