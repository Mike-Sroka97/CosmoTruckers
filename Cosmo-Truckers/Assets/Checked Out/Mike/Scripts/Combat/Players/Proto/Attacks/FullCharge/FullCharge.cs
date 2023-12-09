using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCharge : CombatMove
{
    [SerializeField] DebuffStackSO megawatt;

    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        //Calculate Augment Stacks
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;

        //Apply augment
        CombatManager.Instance.GetCurrentPlayer.AddDebuffStack(megawatt, augmentStacks);
        CombatManager.Instance.GetCurrentPlayer.AddDebuffStack(DebuffToAdd, 2); //always two on device in use
    }
}
