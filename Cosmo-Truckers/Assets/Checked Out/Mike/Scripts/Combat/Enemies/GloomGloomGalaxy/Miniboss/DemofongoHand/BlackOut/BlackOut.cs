using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut : CombatMove
{
    public int MaxNumberOfCycles = 3;
    [HideInInspector] public int NumberOfCycles = 0;

    BlackOutBall[] hands;

    int lastRandom = -1;

    private void Start()
    {
        StartMove();

        hands = GetComponentsInChildren<BlackOutBall>();

        SelectHand();
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
}
