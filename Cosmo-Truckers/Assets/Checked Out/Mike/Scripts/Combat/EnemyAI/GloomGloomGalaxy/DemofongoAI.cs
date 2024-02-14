using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemofongoAI : Enemy
{
    public override void StartTurn()
    {
        int random = Random.Range(0, attacks.Length);

        ChosenAttack = attacks[random];

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Painful Presents and Heavy Weight
        if(attackIndex == 0 || attackIndex == 3)
        {
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);

            if (attackIndex == 3)
                CombatManager.Instance.GetCharactersSelected.Add(this);
        }
        //Switch Master
        else if(attackIndex == 1)
        {
            //Find DPS or random
            if (CombatManager.Instance.FindDPSCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindDPSCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Gun of the maw
        else
        {
            //Add Support or Random
            if (CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
    }
}
