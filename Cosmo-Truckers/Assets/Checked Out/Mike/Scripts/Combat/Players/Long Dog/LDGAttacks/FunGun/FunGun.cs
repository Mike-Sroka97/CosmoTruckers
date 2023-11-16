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
        //defending/attacking
        if (!defending)
            currentDamage = Score * Damage;
        else
            currentDamage = maxScore * Damage - Score * Damage;

        currentDamage += baseDamage;

        //TODO CHANCE add array of augments to dish out in base combat
        //Calculate Augment Stacks
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;

        if (currentDamage > 0)
            CombatManager.Instance.GetCharactersSelected[0].TakeDamage(currentDamage);

        //Apply augment
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd, augmentStacks);
    }
}
