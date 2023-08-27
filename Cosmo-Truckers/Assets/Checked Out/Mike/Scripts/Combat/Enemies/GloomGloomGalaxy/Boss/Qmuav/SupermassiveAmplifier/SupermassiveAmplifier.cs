using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifier : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
    }
}
