using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EclipseEvent : EventNodeBase
{
    public void Stare()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        IgnoreOption();
    }
}
