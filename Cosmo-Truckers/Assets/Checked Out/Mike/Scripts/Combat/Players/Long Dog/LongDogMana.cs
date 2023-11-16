using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogMana : Mana
{
    [Header("basic = 0, gold = 1, stimulant = 2")]
    const int clipSize = 5;
    public List<int> LoadedBullets = new List<int>();
    public List<int> ReserveBullets = new List<int>();

    public override void CheckCastableSpells()
    {
        if (freeSpells)
        {
            foreach (LongDogAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            foreach (LongDogAttackSO attack in attacks)
            {
                //Checks bullets
                if (LoadedBullets.Count < attack.RequiredBullets)
                {
                    attack.CanUse = false;
                    continue;
                }

                attack.CanUse = true;
            }
        }
    }

    public void Shoot(int numberOfBullets = 1)
    {

    }

    public void Reload()
    {

    }

    public void AddBulletsToReserve()
    {

    }
}
