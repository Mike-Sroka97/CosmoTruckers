using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTAttackUI : AttackUI
{
    const int rageToAngerRatio = 3;

    public override void HandleMana()
    {
        SafeTMana mana = myCharacter.GetComponent<SafeTMana>();
        SafeTAttackSO tempAttack = (SafeTAttackSO)currentPlayer.GetAllAttacks[currentAttack];
        int angerConversion = -(tempAttack.RageRequirement * rageToAngerRatio);
        mana.SetCurrentAnger(angerConversion);
    }
}
