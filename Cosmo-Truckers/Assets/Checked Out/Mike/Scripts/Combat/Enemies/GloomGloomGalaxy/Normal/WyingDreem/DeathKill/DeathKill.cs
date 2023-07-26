using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKill : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
