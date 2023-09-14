using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatTonguerAttackSO : BaseAttackSO
{
    public override void StartCombat()
    {
        CombatManager.Instance.GetCurrentEnemy.GetComponent<ENEMY_MeatTonguer>().CharacterInMe = CombatManager.Instance.GetCharactersSelected[0].GetComponent<PlayerCharacter>();
    }
}
