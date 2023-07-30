using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabies : CombatMove
{
    public float FireDelay;

    int numberOfBubbles;
    const int scoreDiff = 2;

    private void Start()
    {
        StartMove();
        GenerateLayout();

        numberOfBubbles = FindObjectsOfType<BubbleBabiesBubble>().Length;
    }

    private void Update()
    {
        TrackTime();
    }

    public override void EndMove()
    {
        int excessScore = 0;
        while(numberOfBubbles > scoreDiff)
        {
            excessScore++;
            numberOfBubbles--;
        }

        Score -= excessScore;

        base.EndMove();
    }
}
