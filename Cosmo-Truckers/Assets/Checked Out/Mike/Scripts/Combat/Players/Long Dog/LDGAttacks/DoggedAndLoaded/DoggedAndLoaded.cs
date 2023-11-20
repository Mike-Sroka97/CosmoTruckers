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
            switch (mana.LoadedBullets[0])
            {
                case 0:
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage(currentDamage, true);
                    break;
                case 1:
                    currentDamage = mana.GoldBulletDamageAdjustment(currentDamage);
                    CombatManager.Instance.GetCharactersSelected[0].TakeDamage(currentDamage, true);
                    break;
                case 2:
                    //TODO CHANCE ADD 2 STACKS OF LONGEVITY
                    CombatManager.Instance.GetCharactersSelected[0].TakeHealing(currentDamage, true);
                    break;
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
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitDamage(currentDamage, 2, true);
                    break;
                case 1:
                    currentDamage = mana.GoldBulletDamageAdjustment(currentDamage);
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitDamage(currentDamage, 2, true);
                    break;
                case 2:
                    //TODO CHANCE ADD 2 STACKS OF LONGEVITY
                    CombatManager.Instance.GetCharactersSelected[0].TakeMultiHitHealing(currentDamage, 2, true);
                    break;
            }
            mana.Shoot(2);
        }

    }

    private void HandleBulletTypes()
    {

    }
}
