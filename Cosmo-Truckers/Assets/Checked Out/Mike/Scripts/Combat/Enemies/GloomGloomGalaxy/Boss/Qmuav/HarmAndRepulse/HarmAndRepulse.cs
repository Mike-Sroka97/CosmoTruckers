using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmAndRepulse : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        base.StartMove();
    }
}
