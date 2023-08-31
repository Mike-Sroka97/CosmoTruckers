using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return (new CaseInsensitiveComparer()).Compare(-((CharacterStats)x).Reflex, -((CharacterStats)y).Reflex);
    }
}
