using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_SpikyShield : Augment
{
    [SerializeField] int shieldAmount = 35;

    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
            return;

        base.Activate(stack);
        AugmentSO.MyCharacter.Shield += shieldAmount;
    }

    public override void StopEffect()
    {
        foreach (CosmicCrustAI cc in FindObjectsOfType<CosmicCrustAI>())
            cc.QueueNextMove();

        AugmentSO.MyCharacter.Shield = 0;
    }

    public override void Trigger()
    {
        if(CombatManager.Instance.AttackingCharacter != AugmentSO.MyCharacter)
            CombatManager.Instance.AttackingCharacter.TakeDamage((int)StatusEffect);

        if (AugmentSO.MyCharacter.Shield <= 0)
            StopEffect();
    }
}
