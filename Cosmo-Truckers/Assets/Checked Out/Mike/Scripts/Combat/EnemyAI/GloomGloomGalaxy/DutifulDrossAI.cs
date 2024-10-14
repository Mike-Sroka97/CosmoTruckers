using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutifulDrossAI : Enemy
{
    [HideInInspector] public Enemy ProtectedEnemy;

    protected override int SelectAttack()
    {
        CurrentTargets.Clear();

        if (ProtectedEnemy && !ProtectedEnemy.Dead)
        {
            bool maxCrust = false;

            foreach (AugmentStackSO aug in ProtectedEnemy.GetAUGS)
                if (aug.DebuffName == "Crust" && aug.CurrentStacks == aug.MaxStacks)
                    maxCrust = true;

            if (maxCrust)
                ChosenAttack = attacks[1];
            else
                ChosenAttack = attacks[1];
        }
        else
        {
            ChosenAttack = attacks[1];
        }

        return GetAttackIndex();
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
            CurrentTargets.Add(this);
        }
    }
}
