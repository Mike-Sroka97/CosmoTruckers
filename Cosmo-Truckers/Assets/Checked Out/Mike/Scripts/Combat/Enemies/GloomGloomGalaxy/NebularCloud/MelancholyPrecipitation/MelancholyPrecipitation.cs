using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitation : CombatMove
{
    [SerializeField] float maxScoreTime;

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

        if((currentTime >= maxScoreTime || PlayerDead) && !MoveEnded)
        {
            if(currentTime >= maxScoreTime)
            {
                currentTime = maxScoreTime;
            }
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
