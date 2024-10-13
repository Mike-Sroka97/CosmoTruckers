using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyNuissance : EventNodeBase
{
    public void ListenToTimmy()
    {
        currentCharacter.TakeHealing(5);
        currentCharacter.AddDebuffStack(augmentsToAdd[0], 2);
        IgnoreOption();
    }

    public void EscapeTimmy()
    {
        currentCharacter.AddDebuffStack(augmentsToAdd[0]);
        IgnoreOption();
    }
}
