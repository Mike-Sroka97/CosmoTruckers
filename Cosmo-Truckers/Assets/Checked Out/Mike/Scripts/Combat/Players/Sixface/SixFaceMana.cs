using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixFaceMana : Mana
{
    public enum FaceTypes
    { 
        Smug,
        Megalomanic,
        Sad,
        Hype,
        Dizzy,
        Money
    }

    public FaceTypes FaceType;

    public override void CheckCastableSpells()
    {
        foreach (SixFaceAttackSO attack in attacks)
        {
            attack.CanUse = true;
        }
    }
}
