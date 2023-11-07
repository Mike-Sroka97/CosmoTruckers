using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixFaceAttackUI : AttackUI
{
    public override void HandleMana()
    {
        SixFaceMana mana = myCharacter.GetComponent<SixFaceMana>();
        SixFaceAttackSO tempAttack = (SixFaceAttackSO)currentPlayer.GetAllAttacks[currentAttack];
        mana.FaceType = tempAttack.faceType;
    }
}
