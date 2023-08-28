using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuarded : CombatMove
{
    void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
