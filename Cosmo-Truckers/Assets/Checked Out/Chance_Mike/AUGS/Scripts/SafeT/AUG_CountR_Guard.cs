using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_CountR_Guard : Augment
{
    int currentShield = 0;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        //Get the current shield level of character at start of combat
        currentShield = stack.MyCharacter.Shield;
    }

    public override void StopEffect()
    {
        //Nothing to remove or reset for this AUG right now
    }

    public override void Trigger()
    {
        //Only call damage if player has shield and it was delt damage
        if(currentShield > 0 && currentShield > AugmentSO.MyCharacter.Shield)
        {
            CombatManager.Instance.GetCurrentEnemy.TakeDamage(currentShield - AugmentSO.MyCharacter.Shield);

            //Remove after activation
            AugmentSO.MyCharacter.AugmentsToRemove.Add(AugmentSO);
        }
    }
}
