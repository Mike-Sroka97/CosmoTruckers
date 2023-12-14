using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_SeedSprout : Augment
{
    bool firstTurn = true;
    int numberOfAugsToRemove = 0;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        numberOfAugsToRemove = (int)StatusEffect;

        if (!firstTurn)
            AugmentSO.MyCharacter.RemoveDebuffStack(AugmentSO);
        else
            firstTurn = false;
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.RemoveAmountOfAugments(numberOfAugsToRemove, 0);
    }
}
