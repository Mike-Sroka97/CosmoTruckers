using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliteBonk : CombatMove
{
    public override void StartMove()
    {
        GetComponentInChildren<Animator>().enabled = true;
        base.StartMove();
    }
}
