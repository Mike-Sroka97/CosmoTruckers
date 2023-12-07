using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_DeviceInUse : Augment
{
    const int fullCharge = 4;

    public override void StopEffect()
    {
        //Add mega watt
        ProtoMana mana = FindObjectOfType<ProtoMana>();
        int batteryOverrage = mana.CurrentBattery;

        //Recharge battery
        mana.UpdateMana(fullCharge);
    }
}
