using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutifulDrossAI : Enemy
{
    [HideInInspector] public Enemy ProtectedEnemy;

    public override void StartTurn()
    {
        if(!ProtectedEnemy.Dead)
        {
            bool maxCrust = false;

            foreach (DebuffStackSO aug in ProtectedEnemy.GetAUGS)
                if (aug.DebuffName == "Crust" && aug.CurrentStacks == aug.MaxStacks)
                    maxCrust = true;

            if(maxCrust)
                ChosenAttack = attacks[1];
            else
                ChosenAttack = attacks[1];
        }
        else
        {
            ChosenAttack = attacks[1];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        if (attackIndex == 0)
        {
            CombatManager.Instance.CharactersSelected.Add(ProtectedEnemy);

            //Add Support or Random
            if (CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }

        //Cometkaze always targets randomly
        else
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
    }
}
