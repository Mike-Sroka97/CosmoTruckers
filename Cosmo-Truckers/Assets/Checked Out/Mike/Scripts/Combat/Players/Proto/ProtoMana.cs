using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMana : Mana
{
    ProtoVessel protoVessel;
    public int CurrentBattery = 0;
    public int CurrentRetention = 0;
    public bool ResetRetention = false;
    public bool InUse = false;

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
                FullChargeCheck(attack);

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

    private void FullChargeCheck(ProtoAttackSO attack)
    {
        if (attack.AttackName == "Full Charge" && InUse)
            attack.BatteryCost = 5; //more mana than Proto will ever have
        else if (attack.AttackName == "Full Charge")
            attack.BatteryCost = CurrentBattery;
    }

    public void UpdateMana(int adjuster)
    {
        int tempBattery = CurrentBattery;

        //update mana
        CurrentBattery += adjuster;

        if (CurrentBattery < CurrentRetention)
            CurrentBattery = CurrentRetention;

        if (CurrentBattery > maxBattery)
            CurrentBattery = maxBattery;
        else if (CurrentBattery < 0)
            CurrentBattery = 0;

        //Triggers Grounded if you had more than 0 battery 
        if(CurrentBattery == 0 && tempBattery > 0)
            foreach (DebuffStackSO aug in myCharacter.GetAUGS)
                if (aug.name == "Grounded(Clone)")
                {
                    aug.DebuffEffect();
                    break;
                }

        MyVessel.ManaTracking();
    }

    public override void SetVessel(PlayerVessel newVessel)
    {
        base.SetVessel(newVessel);
        protoVessel = MyVessel.GetComponent<ProtoVessel>();
    }

    public void AdjustRetention(int newRetention)
    {
        if (newRetention > CurrentBattery)
            CurrentRetention = CurrentBattery;
        else
            CurrentRetention = newRetention;

        MyVessel.ManaTracking();
    }

    public void ClearRetention()
    {
        ResetRetention = false;
        CurrentRetention = 0;

        MyVessel.ManaTracking();
    }
}
