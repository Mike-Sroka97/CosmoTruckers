using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitation : CombatMove
{
    [SerializeField] float oneScoreTime = 9f;
    [SerializeField] float twoScoreTime = 12f;
    [SerializeField] float maxScoreTime = 15f;

    bool trackTime = false;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        MelancholyPrecipitationRaindrop[] raindrops = GetComponentsInChildren<MelancholyPrecipitationRaindrop>();
        foreach (MelancholyPrecipitationRaindrop raindrop in raindrops)
            raindrop.Initialize();
        
        GetComponentInChildren<MelancholyPrecipitationLayoutMovement>().Initialize();
        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }

    protected override void TrackTime()
    {
        if (MoveEnded || !trackTime)
            return;

        base.TrackTime();

        if(currentTime >= maxScoreTime)
        {
            if(currentTime >= maxScoreTime)
            {
                currentTime = maxScoreTime;
            }
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;

        AugmentScore = (int)currentTime;

        if (AugmentScore >= maxScoreTime)
            AugmentScore = 0;
        else if (AugmentScore >= twoScoreTime)
            AugmentScore = 1;
        else if (AugmentScore >= oneScoreTime)
            AugmentScore = 2;

        AugmentScore += baseAugmentStacks;
        int damage = baseDamage;

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
        ApplyAugment(CombatManager.Instance.GetCharactersSelected[0], AugmentScore);
    }
}
