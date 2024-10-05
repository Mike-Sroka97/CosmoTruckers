using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LongDogMana : Mana
{
    [Header("basic = 0, gold = 1, stimulant = 2")]
    public float GoldBulletDamageMultiplier = 1.5f;
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

                //Updates targeting materials based on loaded bullet [0]
                if(attack.RequiredBullets > 0)
                {
                    if (LoadedBullets[0] == 2)
                    {
                        attack.FriendlyPositiveEffect = true;
                        attack.EnemyPositiveEffect = true;
                    }
                    else
                    {
                        attack.FriendlyPositiveEffect = false;
                        attack.EnemyPositiveEffect = false;
                    }
                }

                //Special Cases
                SpecialCaseDoggedAndReloaded(attack);

                attack.CanUse = true;
            }
        }
    }

    private void SpecialCaseDoggedAndReloaded(LongDogAttackSO attack)
    {
        if(attack.AttackName == "Dogged & Loaded")
        {
            if (ReserveBullets.Count <= 0)
            {
                return; 
            }
            if (ReserveBullets[0] == 2)
            {
                attack.FriendlyPositiveEffect = true;
                attack.EnemyPositiveEffect = true;
            }
            else
            {
                attack.FriendlyPositiveEffect = false;
                attack.EnemyPositiveEffect = false;
            }
        }
    }

    public void Shoot(int numberOfBullets = 1)
    {
        for(int i = 0; i < numberOfBullets; i++)
        {
            if(LoadedBullets.Count > 0)
                LoadedBullets.Remove(0);
        }

        MyVessel.GetComponent<LongDogVessel>().DisplayBullets();
    }

    public void Reload()
    {
        LoadedBullets = ReserveBullets.ToList();
        ReserveBullets.Clear();
        MyVessel.GetComponent<LongDogVessel>().DisplayBullets();
    }

    public void AddBulletsToReserve(int numberOfBullets = 1, int typeOfbullet = 0)
    {
        if(ReserveBullets.Count + numberOfBullets > clipSize)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                if (ReserveBullets.Count < clipSize)
                    ReserveBullets.Add(typeOfbullet);
                else
                {
                    ReserveBullets.RemoveAt(0);
                    ReserveBullets.Add(typeOfbullet);
                }
            }
        }
        else
        {
            for (int i = 0; i < numberOfBullets; i++)
                ReserveBullets.Add(typeOfbullet);
        }

        MyVessel.GetComponent<LongDogVessel>().DisplayBullets();
    }

    public int GoldBulletDamageAdjustment(int damage)
    {
        float tempDamage = damage;
        tempDamage *= GoldBulletDamageMultiplier;
        tempDamage = Convert.ToInt32(tempDamage);
        damage = (int)tempDamage;
        return damage;
    }

    public override void SetMaxMana()
    {
        ReserveBullets.Clear();
        LoadedBullets.Clear();

        for(int i = 0; i < 5; i++)
        {
            ReserveBullets.Add(0);
            LoadedBullets.Add(0);
        }

        MyVessel.GetComponent<LongDogVessel>().DisplayBullets();
    }

    public override void ResetMana()
    {
        ReserveBullets.Clear();
        LoadedBullets.Clear();

        for (int i = 0; i < 5; i++)
        {
            LoadedBullets.Add(0);
        }

        MyVessel.GetComponent<LongDogVessel>().DisplayBullets();
    }
}
