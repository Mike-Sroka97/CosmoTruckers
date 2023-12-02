using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAttackUI : AttackUI
{
    public override void HandleMana()
    {
        ProtoMana mana = myCharacter.GetComponent<ProtoMana>();
        ProtoAttackSO tempAttack = (ProtoAttackSO)currentPlayer.GetAllAttacks[currentAttack];
        mana.UpdateMana(-tempAttack.BatteryCost);
    }
}
