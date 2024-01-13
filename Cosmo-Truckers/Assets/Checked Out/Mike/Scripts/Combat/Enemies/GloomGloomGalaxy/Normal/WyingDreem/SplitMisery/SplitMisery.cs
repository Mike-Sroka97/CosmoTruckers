using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMisery : CombatMove
{
    [SerializeField] int nitemareDamageModifier = 2;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();
        bool singlePlayer = players.Length == 1;

        SplitMiserySpike[] spikes = GetComponentsInChildren<SplitMiserySpike>();
        SplitMiserySkeleton[] skeletons = GetComponentsInChildren<SplitMiserySkeleton>();

        foreach (SplitMiserySpike spike in spikes)
            spike.Initialize();

        foreach (SplitMiserySkeleton skeleton in skeletons)
            skeleton.Initialize(singlePlayer);

        trackTime = true;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        //Player 1 damage and augment
        int damage = CalculateScore();

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd, baseAugmentStacks);

        //Player Two Damage
        if (CombatManager.Instance.GetCharactersSelected.Count == 1)
            return;


        int stacksOfNitemare = 0;

        foreach (DebuffStackSO aug in CombatManager.Instance.GetCharactersSelected[0].GetAUGS)
            if (aug.DebuffName == DebuffToAdd.DebuffName)
                stacksOfNitemare = aug.CurrentStacks;

        int scaledDamage = damage + (nitemareDamageModifier * stacksOfNitemare);
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[1], scaledDamage);
    }
}
