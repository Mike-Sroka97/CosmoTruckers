using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbnusAI : Enemy
{
    [SerializeField] int baseDefense = 25;

    [HideInInspector] public bool EyeOneKilled = false;
    [HideInInspector] public bool EyeTwoKilled = false;
    [HideInInspector] public bool EyeThreeKilled = false;

    public int Phase = 1;

    protected override int SelectAttack()
    {
        CurrentTargets.Clear();

        ChosenAttack = attacks[Phase - 1];

        return GetAttackIndex();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Behold, Death!
        if (attackIndex == 0)
        {
            CombatManager.Instance.AOETargetPlayers(this, ChosenAttack);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Last Resort
        else
        {
            CombatManager.Instance.AOETargetPlayers(this, ChosenAttack);
        }
    }

    public void ShredArmor()
    {
        TakeDamage(1000, true);
        Phase = 2;
        Stats.Defense = baseDefense;
    }

    public override void Die()
    {
        Phase = 3;
    }

    public void DieForReal()
    {
        base.Die();
    }
}
