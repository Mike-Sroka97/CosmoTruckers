using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreem : CombatMove
{
    [SerializeField] int startingScore;
    [SerializeField] float maxTime;

    float currentTime = 0;
    [HideInInspector] public float CurrentScore;

    private void Start()
    {
        StartMove();
        GenerateLayout();

        CurrentScore = startingScore;
    }

    private void Update()
    {
        TrackScore();
    }

    private void TrackScore()
    {
        if (MoveEnded)
            return;

        currentTime += Time.deltaTime;

        if(CurrentScore < 0.5f || currentTime >= maxTime)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;
        CurrentScore += 0.5f;
        Score = (int)CurrentScore;
    }
}
