using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomyGloop : EventNodeBase
{
    public void GetGlooped()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }
}
