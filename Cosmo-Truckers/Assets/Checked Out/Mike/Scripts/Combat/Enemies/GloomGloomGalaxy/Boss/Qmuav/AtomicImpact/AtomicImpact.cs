using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpact : CombatMove
{
    [SerializeField] int numberOfHits = 3;

    public override void StartMove()
    {
        GetComponentInChildren<GravityManager>().Initialize();

        GenerateLayout();
        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        DealMultiHitDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], CalculateScore(), numberOfHits); 
    }
}
 