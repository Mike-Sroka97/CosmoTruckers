using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpact : CombatMove
{
    [SerializeField] int numberOfHits = 3;

    public override void StartMove()
    {
        GenerateNextWave();
        base.StartMove();
    }

    public void GenerateNextWave()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        DealMultiHitDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], CalculateScore(), numberOfHits); 
    }
}
