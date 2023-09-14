using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY_MeatTonguer : Enemy
{
    [HideInInspector] public PlayerCharacter CharacterInMe;

    public override void TakeDamage(int damage)
    {
        if(CombatManager.Instance.GetCurrentPlayer != CharacterInMe)
        {
            damage = AdjustAttackDamage(damage);
        }

        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
            passiveMove.Activate(CurrentHealth);

        if (Shield > 0) Shield = Shield - damage <= 0 ? 0 : Shield - damage;
        else CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
        //See if any AUGS trigger on Damage (Spike shield)
        else
        {
            foreach (DebuffStackSO aug in AUGS)
            {
                if (aug.Type == DebuffStackSO.ActivateType.OnDamage)
                {
                    aug.GetAugment().Trigger();
                }
            }
        }
    }
}
