using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryWalkingNoise : EventNodeBase
{
    public void NoOption()
    {
        StartCoroutine(SelectionChosen());
    }
}
