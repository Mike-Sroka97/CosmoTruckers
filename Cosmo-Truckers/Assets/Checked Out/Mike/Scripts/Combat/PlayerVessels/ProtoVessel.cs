using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoVessel : PlayerVessel
{
    [SerializeField] Image[] batteryPips;
    [SerializeField] Sprite normalCharge;
    [SerializeField] Sprite retainedCharge;

    ProtoMana protoMana;

    const int maxBattery = 4;

    public override void Initialize(PlayerCharacter player)
    {
        base.Initialize(player);
        ManaTracking();
    }

    public override void ManaTracking()
    {
        //reset mana
        foreach (Image node in batteryPips)
            node.color = Color.clear;

        //set current mana
        protoMana = MyMana.GetComponent<ProtoMana>();
        int batteryCount = protoMana.CurrentBattery;
        int retentionCount = protoMana.CurrentRetention;

        for (int i = 0; i < maxBattery; i++)
        {
            if(i < retentionCount)
            {
                batteryPips[i].color = Color.white;
                batteryPips[i].sprite = retainedCharge;
            }
            else if (i < batteryCount)
            {
                batteryPips[i].color = Color.white;
                batteryPips[i].sprite = normalCharge;
            }
            else
            {
                batteryPips[i].color = Color.clear;
            }
        }
    }
}
