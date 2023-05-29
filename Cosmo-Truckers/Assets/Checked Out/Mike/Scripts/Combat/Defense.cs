using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatMove;

public class Defense : CombatMove
{
    [SerializeField] TargetType TypeOfAttack;
    public void ApplyAugments()
    {
        //Applies to all targeted players (will have to consider how augments are combined)
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
