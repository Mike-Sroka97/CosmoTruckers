using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface CombatMove
{
    public enum TargetType
    {
        NoTarget,
        SelfTarget,
        SingleTarget,
        MultiTarget,
        Cone,
        MultiTargetChoice,
        AOE,
        AllTarget
    }

    void StartMove();
    void EndMove();
    void ApplyAugments();
}
