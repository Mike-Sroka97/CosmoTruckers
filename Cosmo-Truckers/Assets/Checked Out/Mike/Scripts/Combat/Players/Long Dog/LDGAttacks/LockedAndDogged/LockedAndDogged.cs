using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedAndDogged : CombatMove
{
    private void Start()
    {
        StartMove();
    }
    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
