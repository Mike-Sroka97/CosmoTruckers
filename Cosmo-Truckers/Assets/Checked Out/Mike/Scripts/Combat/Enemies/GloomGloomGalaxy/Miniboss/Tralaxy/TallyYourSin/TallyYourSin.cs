using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSin : CombatMove
{
    [SerializeField] Animator mickeysDickMasher;
    [SerializeField] DebuffStackSO sorrow;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        base.StartMove();

        foreach (TallyYourSinCircle sin in GetComponentsInChildren<TallyYourSinCircle>())
            sin.Initialize();

        foreach (TallyYourSinSin sin in GetComponentsInChildren<TallyYourSinSin>())
            sin.Initialize();

        mickeysDickMasher.enabled = true;

        base.StartMove();
    }

    public override void EndMove()
    {
        base.EndMove();
        int sorrowStacks = CalculateAugmentScore();

        foreach (DebuffStackSO aug in CombatManager.Instance.GetCharactersSelected[0].GetAUGS)
            if (aug.DebuffName == "Sin")
                sorrowStacks += aug.CurrentStacks;

        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(sorrow, sorrowStacks);
    }
}
