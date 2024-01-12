using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encrustable : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    private void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        int stacks = CalculateAugmentScore();

        ApplyAugment(CombatManager.Instance.CharactersSelected[0], stacks);
    }
}
