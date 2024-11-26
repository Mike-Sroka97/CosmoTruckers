using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_CountR_Guard : Augment
{
    int currentShield = 0;
    [SerializeField] int damageToDeal = 2; 

    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);
    }

    public override void StopEffect()
    {
        // no-op
    }

    public override void Trigger()
    {
        if (CombatManager.Instance.AttackingCharacter == AugmentSO.MyCharacter)
            return;

        // Damage the enemy every time SafeT is hit when they have Count-R Guard
        CombatManager.Instance.AttackingCharacter.TakeDamage(damageToDeal);

        // Give SafeT one ang-r every time SafeT hits the enemy
        FindObjectOfType<SafeTMana>().SetCurrentAnger(1);
    }
}
