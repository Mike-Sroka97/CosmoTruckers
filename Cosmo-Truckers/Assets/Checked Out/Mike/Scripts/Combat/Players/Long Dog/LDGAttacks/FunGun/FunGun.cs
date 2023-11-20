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

        LongDogMana mana = FindObjectOfType<LongDogMana>();
        if (currentDamage > 0)
        {
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
        }

        mana.Shoot();
    }
}
