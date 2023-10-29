using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarMana : Mana
{
    public int VeggieMana = 0;
    public int MeatMana = 0;
    public int SweetMana = 0;

    const int maxMana = 9;

    public override void CheckCastableSpells()
    {
        if (freeSpells)
        {
            foreach (AeglarAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            //alive check for rez??

            foreach (AeglarAttackSO attack in attacks)
            {
                if (attack.VeggieRequirement <= VeggieMana
                    && attack.MeatRequirement <= MeatMana
                    && attack.SweetRequirement <= SweetMana)
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

    public void AdjustMana(int manaAmount, int manaIndex)
    {
        if(manaIndex == 0)
        {
            VeggieMana += manaAmount;
            if (VeggieMana < 0)
                VeggieMana = 0;
            else if (VeggieMana > maxMana)
                VeggieMana = maxMana;
        }
        else if (manaIndex == 1)
        {
            MeatMana += manaAmount;
            if (MeatMana < 0)
                MeatMana = 0;
            else if (MeatMana > maxMana)
                MeatMana = maxMana;
        }
        else
        {
            SweetMana += manaAmount;
            if (SweetMana < 0)
                SweetMana = 0;
            else if (SweetMana > maxMana)
                SweetMana = maxMana;
        }

        MyVessel.ManaTracking();
    }
}
