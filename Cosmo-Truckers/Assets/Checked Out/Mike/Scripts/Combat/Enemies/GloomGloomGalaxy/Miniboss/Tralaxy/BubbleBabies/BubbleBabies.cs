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
        GenerateLayout();

        numberOfBubbles = FindObjectsOfType<BubbleBabiesBubble>().Length;
    }

    public override void StartMove()
    {
        base.StartMove();

        BubbleBabiesNeedle[] needles = GetComponentsInChildren<BubbleBabiesNeedle>();

        foreach (BubbleBabiesNeedle needle in needles)
            needle.Initialize();

        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        Score = numberOfBubbles / 2;
        Score++;

        int currentBubbles = 0;

        for(int i = 8; i <= 11; i++)
        {
            if(EnemyManager.Instance.EnemyCombatSpots[i] != null && !EnemyManager.Instance.EnemyCombatSpots[i].Dead)
            {
                EnemyManager.Instance.EnemyCombatSpots[i].AdjustBubbleShield(true);
                currentBubbles++;
                if (currentBubbles > Score)
                    return;
            }
        }
    }
}
