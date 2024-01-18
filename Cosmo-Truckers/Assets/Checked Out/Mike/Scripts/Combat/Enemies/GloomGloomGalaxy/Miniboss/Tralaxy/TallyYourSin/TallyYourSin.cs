using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyYourSin : CombatMove
{
    [SerializeField] Animator mickeysDickMasher;

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
    }
}
