using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMaw : CombatMove
{
    [SerializeField] float oneScoreTime = 9f;
    [SerializeField] float twoScoreTime = 12f;
    [SerializeField] float maxScoreTime = 15f;

    public override void StartMove()
    {
        GetComponentInChildren<GunOfTheMawHead>().enabled = true;
        GetComponentInChildren<GunOfTheMawGun>().enabled = true;
        trackTime = true;

        base.StartMove();
    }

    protected override void TrackTime()
    {
        if (MoveEnded || !trackTime)
            return;

        base.TrackTime();

        if (currentTime >= maxScoreTime)
        {
            if (currentTime >= maxScoreTime)
            {
                currentTime = maxScoreTime;
            }
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true; 

        int DamageToAdd = 0;
        int AugmentStacksToRemove = maxAugmentStacks;

        // Min success
        if (currentTime <= oneScoreTime)
        {
            DamageToAdd = Damage;
            AugmentStacksToRemove += augmentStacksPerScore * 2;
        }
        // Mid success
        if (currentTime <= twoScoreTime && currentTime > oneScoreTime)
        {
            DamageToAdd = Damage / 2;
            AugmentStacksToRemove += augmentStacksPerScore; 
        }
        // Max success
        else if (currentTime >= maxScoreTime)
        {
            DamageToAdd = 0;
        }

        DamageToAdd += baseDamage; 

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], DamageToAdd);
        CombatManager.Instance.GetCharactersSelected[1].RemoveAmountOfAugments(AugmentStacksToRemove, 0);
    }
}
