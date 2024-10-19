using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberDilemma : EventNodeBase
{
    [SerializeField] int healingAmount = 22;

    public void Ponder()
    {
        currentCharacter.TakeHealing(healingAmount);
        AddAugmentToPlayer(augmentsToAdd[0], 4);
        IgnoreOption();
    }
}
