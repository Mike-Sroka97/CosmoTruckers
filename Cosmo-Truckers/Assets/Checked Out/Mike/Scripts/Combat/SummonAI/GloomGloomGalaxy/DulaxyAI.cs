using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DulaxyAI : PlayerCharacterSummon
{
    [SerializeField] int healthPerTurn;

    public int HealAmount = 0;

    public override void StartTurn()
    {
        HealAmount += healthPerTurn;
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex) 
    {
        CombatManager.Instance.SelectRandomPlayerCharacter(true);
    }
}
