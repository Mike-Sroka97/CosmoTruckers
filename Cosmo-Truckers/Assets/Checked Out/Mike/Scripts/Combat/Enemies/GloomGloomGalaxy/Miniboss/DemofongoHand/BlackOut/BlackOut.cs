using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut : CombatMove
{
    [SerializeField] GameObject fauxPaw;

    public int MaxNumberOfCycles = 3;
    [HideInInspector] public int NumberOfCycles = 0;

    BlackOutBall[] hands;

    int lastRandom = -1;
    public override void StartMove()
    {
        base.StartMove();

        hands = GetComponentsInChildren<BlackOutBall>();

        SelectHand();

        base.StartMove();
    }

    public void SelectHand()
    {
        int random = UnityEngine.Random.Range(0, hands.Length);

        while(random == lastRandom)
        {
            random = UnityEngine.Random.Range(0, hands.Length);
        }

        lastRandom = random;

        for (int i = 0; i < hands.Length; i++)
        {
            if(i != random)
            {
                hands[i].ActivateMe(false);
            }
            else
            {
                hands[i].ActivateMe(true);
            }
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if (Score > maxScore)
            Score = maxScore;
        else if (Score < 0)
            Score = 0;

        int pawsToSummon = 4 - Score; 

        for(int i = 0; i < pawsToSummon; i++)
        {
            EnemyManager.Instance.UpdateEnemySummons(fauxPaw);
        }
    }
}
