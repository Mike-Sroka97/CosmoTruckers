using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggedAndLoaded : CombatMove
{
    LongDogMana mana;
    int currentDamage;

    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        //Calculate Damage
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        currentDamage = Score * Damage;

        currentDamage += baseDamage;

        //1 being base damage
        float DamageAdj = 1;
        float HealingAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
        HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

        mana = FindObjectOfType<LongDogMana>();
        //Don't Shoot
        if(mana.ReserveBullets.Count <= 0)
        {
            mana.Reload();
        }
        //Shoot
        else if(mana.ReserveBullets.Count == 1)
        {
            mana.Reload();
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
                        CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(DebuffToAdd, 2);
                        CombatManager.Instance.GetCharactersSelected[0].TakeHealing((int)(currentDamage * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), true);
                        break;
                }
            }
            mana.Shoot();
        }
        //Shoot twice
        else
        {
            mana.Reload();
            switch (mana.LoadedBullets[0])
            {
                case 0:
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), 2, true);
                    break;
                case 1:
                    currentDamage = mana.GoldBulletDamageAdjustment(currentDamage);
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), 2, true);
                    break;
                case 2:
                    CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(DebuffToAdd, 2);
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitHealing((int)(currentDamage * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), 2, true);
                    break;
            }
            mana.Shoot(2);
        }

    }

    private void HandleBulletTypes()
    {

    }
}
