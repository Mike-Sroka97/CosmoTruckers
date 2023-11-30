using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunGun : CombatMove
{
    [SerializeField] Transform layout;

    FGGun[] guns;
    int currentActiveGun;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        guns = FindObjectsOfType<FGGun>();

        foreach (FGGun gun in guns)
            gun.StartMove();

        currentActiveGun = UnityEngine.Random.Range(0, guns.Length);
        guns[currentActiveGun].TrackingTime = true;
    }

    public void NextGun()
    {
        if(currentActiveGun + 1 < guns.Length)
        {
            currentActiveGun++;
        }
        else
        {
            currentActiveGun = 0;
        }

        guns[currentActiveGun].TrackingTime = true;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        //Calculate Damage
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage;
        currentDamage = Score * Damage;

        currentDamage += baseDamage;

        //1 being base damage
        float DamageAdj = 1;
        float HealingAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
        HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

        LongDogMana mana = FindObjectOfType<LongDogMana>();
        if (currentDamage > 0)
        {
            switch (mana.LoadedBullets[0])
            {
                case 0:
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), true);
                    break;
                case 1:
                    currentDamage = mana.GoldBulletDamageAdjustment(currentDamage);
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), true);
                    break;
                case 2:
                    CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd, 2);
                    CombatManager.Instance.GetCharactersSelected[0].TakeHealing((int)(currentDamage * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), true);
                    break;
            }
        }

        mana.Shoot();
    }
}
