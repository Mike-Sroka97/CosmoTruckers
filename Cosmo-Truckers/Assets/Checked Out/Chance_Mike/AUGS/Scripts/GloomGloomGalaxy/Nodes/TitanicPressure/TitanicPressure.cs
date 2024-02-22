using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanicPressure : Augment
{
    [SerializeField] int damagePerTurn;
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        if (firstGo)
            AugmentSO.MyCharacter.AdjustAttackDamage((int)StatusEffect);
        else
            AugmentSO.MyCharacter.TakeDamage(damagePerTurn);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustAttackDamage(-(int)StatusEffect);
    }
}
