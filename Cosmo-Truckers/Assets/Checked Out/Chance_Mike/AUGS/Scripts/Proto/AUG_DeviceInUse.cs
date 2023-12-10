using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_DeviceInUse : Augment
{
    const int fullCharge = 4;

    public override void StopEffect()
    {
        //Recharge battery
        ProtoMana mana = FindObjectOfType<ProtoMana>();
        mana.UpdateMana(fullCharge);
        mana.InUse = false;
    }
}
