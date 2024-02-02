using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavAI : Enemy
{
    public override void StartTurn()
    {
        ChosenAttack = attacks[1];
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Harm and Repulse
        if (attackIndex == 0)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Hive Healing
        else if (attackIndex == 1)
        {
            CombatManager.Instance.GetCharactersSelected.Add(this);
            if(CombatManager.Instance.FindSupportCharacter())
            {
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            }
            else
            {
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            }
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
