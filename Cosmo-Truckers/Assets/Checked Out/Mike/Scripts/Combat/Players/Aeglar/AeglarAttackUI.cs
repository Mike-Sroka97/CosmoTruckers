using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarAttackUI : AttackUI
{
    public override void HandleMana()
    {
        AeglarMana mana = myCharacter.GetComponent<AeglarMana>();
        AeglarAttackSO tempAttack = (AeglarAttackSO)currentPlayer.GetAllAttacks[currentAttack];

        mana.AdjustMana(-tempAttack.VeggieRequirement, 0);
        mana.AdjustMana(-tempAttack.MeatRequirement, 1);
        mana.AdjustMana(-tempAttack.SweetRequirement, 2);
    }
}
