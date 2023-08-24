using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraviticSiphon : CombatMove
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
