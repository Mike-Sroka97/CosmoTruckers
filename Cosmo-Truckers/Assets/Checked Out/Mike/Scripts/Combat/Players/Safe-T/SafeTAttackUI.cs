using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTAttackUI : AttackUI
{
    const int rageToAngerRatio = 3;

    protected override void StartAttack()
    {
        SafeTMana mana = myCharacter.GetComponent<SafeTMana>();
        SafeTAttackSO tempAttack = (SafeTAttackSO)currentPlayer.GetAllAttacks[currentAttack];
        int angerConversion = -(tempAttack.RageRequirement * rageToAngerRatio);
        mana.SetCurrentAnger(angerConversion);

        base.StartAttack();
    }
}
