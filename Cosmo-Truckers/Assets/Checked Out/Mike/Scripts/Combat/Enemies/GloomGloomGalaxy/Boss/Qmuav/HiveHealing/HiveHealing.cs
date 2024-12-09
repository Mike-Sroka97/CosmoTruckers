using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHealing : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        GetComponentInChildren<GravityManager>().Initialize();

        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        //Qmuav
        int healing = CalculateScore();
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], healing);

        //Player
        CombatManager.Instance.GetCharactersSelected[1].AddAugmentStack(DebuffToAdd, baseAugmentStacks);
    }
}
