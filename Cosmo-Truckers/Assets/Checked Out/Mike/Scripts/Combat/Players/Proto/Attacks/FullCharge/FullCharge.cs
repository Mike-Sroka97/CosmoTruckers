using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCharge : CombatMove
{
    [SerializeField] AugmentStackSO megawatt;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();
    }

    public override void EndMove()
    {
        //Calculate Augment Stacks
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;

        //Apply augment
        CombatManager.Instance.GetCurrentPlayer.AddAugmentStack(megawatt, augmentStacks);
        CombatManager.Instance.GetCurrentPlayer.AddAugmentStack(DebuffToAdd, 2); //always two on device in use
        FindObjectOfType<ProtoMana>().InUse = true;
    }
}
