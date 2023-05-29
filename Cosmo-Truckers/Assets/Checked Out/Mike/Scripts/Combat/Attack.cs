using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatMove;

public class Attack : CombatMove
{
    [SerializeField] TargetType TypeOfAttack;
    public void ApplyAugments()
    {
        //Applies to single player
        throw new System.NotImplementedException();
    }

    public void EndMove()
    {
        throw new System.NotImplementedException();
    }

    public void StartMove()
    {
        throw new System.NotImplementedException();
    }
}
