using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_SweetPlacebo : Augment
{
    [SerializeField] int healingAmount = 3;

    public override void Activate(AugmentStackSO stack = null)
    {
        Debug.Log("Sweet Placebo!"); 

        base.Activate(stack);

        //1 being base damage
        float HealingAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

        AugmentSO.MyCharacter.TakeHealing((int)(healingAmount * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment));
    }

    public override void Trigger()
    {
        AugmentSO.MyCharacter.RemoveAugmentStack(AugmentSO);
    }

    public override void StopEffect()
    {
        //LOL
    }
}
