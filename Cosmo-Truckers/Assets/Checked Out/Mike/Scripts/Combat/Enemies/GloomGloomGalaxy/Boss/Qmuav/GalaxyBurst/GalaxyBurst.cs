using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyBurst : CombatMove
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
