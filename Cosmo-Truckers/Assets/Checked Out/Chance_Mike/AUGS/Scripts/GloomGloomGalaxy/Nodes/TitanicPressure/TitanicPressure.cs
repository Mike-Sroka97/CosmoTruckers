using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanicPressure : Augment
{
    [SerializeField] int damagePerTurn;
    public override void Activate(AugmentStackSO stack = null)
    {
        if (firstGo)
        {
            base.Activate(stack);
            AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
        }
        else
            AugmentSO.MyCharacter.TakeDamage(damagePerTurn);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
