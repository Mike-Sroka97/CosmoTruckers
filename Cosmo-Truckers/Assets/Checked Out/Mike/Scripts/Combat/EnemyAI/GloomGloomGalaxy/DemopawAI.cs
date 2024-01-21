using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemopawAI : EnemySummon
{
    [SerializeField] int eyeGougerWeight = 2;
    [SerializeField] int blackOutWeight = 1;

    public override void StartTurn()
    {
        int random = Random.Range(0, 3);

        //eye gouger
        if (EnemyManager.Instance.GetAliveEnemySummons().Count >= 2 || random < eyeGougerWeight)
        {
            ChosenAttack = attacks[0];
        }
        //black out
        else
        {
            ChosenAttack = attacks[1];
        }

        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //eye gouger
        if (attackIndex == 0)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //black out
        else
        {
            if (CombatManager.Instance.FindUtilityCharacter() != null)
                CombatManager.Instance.CharactersSelected.Add(CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.IgnoreTauntSingleTarget(true);
        }
    }
}
