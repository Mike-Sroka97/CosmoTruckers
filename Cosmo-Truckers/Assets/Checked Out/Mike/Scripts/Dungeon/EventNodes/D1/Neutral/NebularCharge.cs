using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebularCharge : EventNodeBase
{
    public void GetShocked()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        AddAugmentToPlayer(augmentsToAdd[1]);
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }
}
