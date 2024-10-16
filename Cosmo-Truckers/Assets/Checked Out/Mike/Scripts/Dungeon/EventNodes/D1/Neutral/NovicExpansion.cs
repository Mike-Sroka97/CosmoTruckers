using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovicExpansion : EventNodeBase
{
    public void ExplosiveExpansion()
    {
        currentCharacter.ProliferateAugment(3, 1);
        currentCharacter.ProliferateAugment(3, 0);
        IgnoreOption();
    }
}
