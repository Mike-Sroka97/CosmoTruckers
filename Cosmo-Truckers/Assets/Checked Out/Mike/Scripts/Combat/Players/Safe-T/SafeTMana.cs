using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTMana : Mana
{
    [Space(20)]
    [Header("Casting Variables")]
    [SerializeField] int maxAnger;
    [SerializeField] int superSlamOneCostHealth = 80;
    [SerializeField] int superSlamZeroCostHealth = 40;

    SafeTVessel safeTVessel;
    SafeTAttackSO superSlam;

    const int threeRage = 9;
    const int twoRage = 6;
    const int oneRage = 3;

    int currentAnger = 0;
    int currentRage = 0;

    public override void CheckCastableSpells()
    {
        if (freeSpells)
        {
            foreach (SafeTAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            SuperSlamCheck();

            foreach (SafeTAttackSO attack in attacks)
            {
                if (attack.RageRequirement <= currentRage)
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

    private void SuperSlamCheck()
    {
        //Super Slam has a variable mana !!!
        superSlam = (SafeTAttackSO)attacks[4];
        if (myCharacter.CurrentHealth > superSlamOneCostHealth)
            superSlam.RageRequirement = 2;
        else if (myCharacter.CurrentHealth <= superSlamZeroCostHealth)
            superSlam.RageRequirement = 0;
        else
            superSlam.RageRequirement = 1;
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

        MyVessel.ManaTracking();
    }

    public override void SetVessel(PlayerVessel newVessel)
    {
        base.SetVessel(newVessel);
        safeTVessel = MyVessel.GetComponent<SafeTVessel>();
    }

    public int GetCurrentAnger() { return currentAnger; }
    public int GetCurrentRage() { return currentRage; }
}
