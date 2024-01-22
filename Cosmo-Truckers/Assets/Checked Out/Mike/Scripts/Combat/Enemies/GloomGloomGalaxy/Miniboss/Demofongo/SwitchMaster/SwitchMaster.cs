using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaster : CombatMove
{
    [SerializeField] SwitchMasterItem masterItem;

    public int MaxNumberOfCycles;
    [HideInInspector] public int CurrentNumberOfCycles;

    public override void StartMove()
    {
        masterItem.ActivateMe();
        base.StartMove();
    }
}
