using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavAI : Enemy
{
    public override void StartTurn()
    {
        ChosenAttack = attacks[0];
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Harm and Repulse
        if (attackIndex == 0)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Titan's Terror
        else if (attackIndex == 1)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
    }
}
