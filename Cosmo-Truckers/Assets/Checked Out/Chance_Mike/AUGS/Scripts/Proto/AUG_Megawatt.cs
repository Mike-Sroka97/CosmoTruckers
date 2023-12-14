using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Megawatt : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        if(CombatManager.Instance.CurrentAttack.CombatPrefab.GetComponent<CombatMove>().GetIsDamaging())
        {
            AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
            AugmentSO.SetFade(AugmentSO.MaxStacks); //makes sure to get rid of aug at the end of the turn
        }
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
