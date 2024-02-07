using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifier : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        foreach (SupermassiveAmplifierEye eye in GetComponentsInChildren<SupermassiveAmplifierEye>())
            eye.enabled = true;

        SetupMultiplayer();

        base.StartMove();
    }
}
