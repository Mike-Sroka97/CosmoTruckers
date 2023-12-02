using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMana : Mana
{
    ProtoVessel protoVessel;
    public int CurrentBattery = 0;

    const int maxBattery = 4;

    public override void CheckCastableSpells()
    {
        if (freeSpells)
        {
            foreach (ProtoAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            foreach (ProtoAttackSO attack in attacks)
            {
                if (attack.BatteryCost <= CurrentBattery)
                {
                    attack.CanUse = true;
                }
                else
                {
                    attack.CanUse = false;
                }
            }
        }
    }
    public void UpdateMana(int adjuster = 3)
    {
        //update mana
        CurrentBattery += adjuster;

        if (CurrentBattery > maxBattery)
            CurrentBattery = maxBattery;
        else if (CurrentBattery < 0)
            CurrentBattery = 0;

        MyVessel.ManaTracking();
    }

    public override void SetVessel(PlayerVessel newVessel)
    {
        base.SetVessel(newVessel);
        protoVessel = MyVessel.GetComponent<ProtoVessel>();
    }
}
