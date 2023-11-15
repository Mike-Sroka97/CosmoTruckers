using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogAttackUI : AttackUI
{
    public override void HandleMana()
    {
        LongDogMana mana = myCharacter.GetComponent<LongDogMana>();
        LongDogAttackSO tempAttack = (LongDogAttackSO)currentPlayer.GetAllAttacks[currentAttack];
        for(int i = 0; i < tempAttack.RequiredBullets; i++)
        {
            mana.loadedBullets.Remove(0);
        }
    }
}
