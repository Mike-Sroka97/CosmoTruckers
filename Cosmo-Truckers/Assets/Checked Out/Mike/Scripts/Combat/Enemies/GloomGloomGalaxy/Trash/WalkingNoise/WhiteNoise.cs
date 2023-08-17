using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
