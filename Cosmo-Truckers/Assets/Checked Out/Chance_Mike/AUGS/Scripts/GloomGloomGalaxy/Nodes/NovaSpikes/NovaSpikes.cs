using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpikes : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
            return;

        base.Activate(stack);
    }

    public override void StopEffect()
    {
        //No-op just stops triggering
    }

    public override void Trigger()
    {
        if(CombatManager.Instance.AttackingCharacter != AugmentSO.MyCharacter)
            CombatManager.Instance.AttackingCharacter.TakeDamage((int)StatusEffect);
    }
}
