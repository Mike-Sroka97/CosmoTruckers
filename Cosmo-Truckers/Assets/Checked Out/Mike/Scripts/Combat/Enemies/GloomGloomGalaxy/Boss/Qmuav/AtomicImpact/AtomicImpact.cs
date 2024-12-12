using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpact : CombatMove
{
    [SerializeField] int numberOfHits = 3;

    [SerializeField] int maxWaveCount = 2;
    int currentWaveCount = 0;

    public override void StartMove()
    {
        GetComponentInChildren<GravityManager>().Initialize();

        currentWaveCount = maxWaveCount; 
        GenerateNextWave();
        base.StartMove();
    }

    public void GenerateNextWave()
    {
        if (currentWaveCount > 0)
        {
            currentWaveCount--;
            GenerateLayout();
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        DealMultiHitDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], CalculateScore(), numberOfHits); 
    }
}
 