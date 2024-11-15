using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSin : CombatMove
{
    [SerializeField] Animator mickeysDickMasher;
    [SerializeField] AugmentStackSO sorrow;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();

        foreach (TallyYourSinCircle sin in GetComponentsInChildren<TallyYourSinCircle>())
            sin.Initialize();

        mickeysDickMasher.enabled = true;
    }

    public override void EndMove()
    {
        base.EndMove();
        int sorrowStacks = CalculateAugmentScore();

        foreach (AugmentStackSO aug in CombatManager.Instance.GetCharactersSelected[0].GetAUGS)
            if (aug.AugmentName == "Sin")
                sorrowStacks += aug.CurrentStacks;

        CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(sorrow, sorrowStacks);
    }
}
