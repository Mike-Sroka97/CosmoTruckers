using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
    }

    protected override void TrackTime()
    {
        if (MoveEnded)
            return;

        currentTime += Time.deltaTime;

        if ((currentTime >= MinigameDuration || PlayerDead) && !MoveEnded)
        {
            Score = 0;
            EndMove();
        }
    }

    public void SetScore()
    {
        Score = (int)currentTime;
        EndMove();
    }
}
