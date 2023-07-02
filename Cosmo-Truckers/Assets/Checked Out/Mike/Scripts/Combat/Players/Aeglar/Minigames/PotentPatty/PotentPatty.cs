using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPatty : CombatMove
{
    private void Start()
    {
        Score = GetComponentsInChildren<PotentPattyPatty>().Length;
        StartMove();
    }
    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
