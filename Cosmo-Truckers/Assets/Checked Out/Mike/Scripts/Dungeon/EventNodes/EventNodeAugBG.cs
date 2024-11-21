using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventNodeAugBG : MonoBehaviour
{
    [SerializeField] Sprite[] bgs;

    public Sprite SetBG(AugmentStackSO aug)
    {

        if (!aug.IsBuff && !aug.IsDebuff && aug.Removable)
            return bgs[0];

        else if (!aug.IsBuff && !aug.IsDebuff && !aug.Removable)
            return bgs[1];

        else if (aug.IsBuff && aug.Removable)
            return bgs[2];

        else if (aug.IsBuff && !aug.Removable)
            return bgs[3];

        else if (aug.IsDebuff && aug.Removable)
            return bgs[4];

        else if (aug.IsDebuff && !aug.Removable)
            return bgs[5];
        else
            return bgs[6];
    }
}
