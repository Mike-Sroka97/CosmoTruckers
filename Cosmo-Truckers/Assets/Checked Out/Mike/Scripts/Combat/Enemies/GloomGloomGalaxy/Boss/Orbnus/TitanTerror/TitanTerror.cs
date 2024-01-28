using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanTerror : CombatMove
{
    public override void StartMove()
    {
        foreach (EyeFollower eye in GetComponentsInChildren<EyeFollower>())
            eye.enabled = true;
        foreach (ORBTitanCrusher crusher in GetComponentsInChildren<ORBTitanCrusher>())
            crusher.enabled = true;

        base.StartMove();
    }
}
