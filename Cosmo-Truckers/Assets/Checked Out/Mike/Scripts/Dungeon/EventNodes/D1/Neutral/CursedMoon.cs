using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedMoon : EventNodeBase
{
    public void StareIntoTheMoon()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        IgnoreOption();
    }
}
