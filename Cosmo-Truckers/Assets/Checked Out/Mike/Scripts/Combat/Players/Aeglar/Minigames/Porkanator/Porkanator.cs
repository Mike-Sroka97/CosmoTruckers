using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porkanator : CombatMove
{

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
