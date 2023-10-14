using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTMana : Mana
{
    [SerializeField] int maxAnger;

    const int threeRage = 9;
    const int twoRage = 6;
    const int oneRage = 3;

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

    public void SetCurrentAnger(int adjuster)
    {
        //never let anger exceed max value
        currentAnger += adjuster;
        if (currentAnger > maxAnger)
        {
            currentAnger = maxAnger;
        }

        UpdateMana();
    }
    private void UpdateMana()
    {
        //update mana
        if (currentAnger == threeRage)
            currentRage = 3;
        else if (currentAnger >= twoRage)
            currentRage = 2;
        else if (currentAnger >= oneRage)
            currentRage = 1;
        else
            currentRage = 0;
    }
}
