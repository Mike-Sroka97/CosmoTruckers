using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTMana : Mana
{
    [SerializeField] int maxAnger;
    [SerializeField] int maxRage;

    int currentAnger = 0;
    int currentRage = 0;

    public override void CheckCastableSpells()
    {
        //Super Slam has a variable mana !!! TODO check myCharacter hp for this!

        foreach(SafeTAttackSO attack in attacks)
        {
            if(attack.RageRequirement <= currentRage)
            {
                attack.CanUse = true;
            }
            else
            {
                attack.CanUse = false;
            }
        }
    }
}
