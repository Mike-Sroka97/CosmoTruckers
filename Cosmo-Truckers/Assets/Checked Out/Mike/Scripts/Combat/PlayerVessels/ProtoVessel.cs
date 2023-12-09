using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoVessel : PlayerVessel
{
    [SerializeField] Image[] batteryPips;
    [SerializeField] Color offColor;
    [SerializeField] Color onColor;
    [SerializeField] Color retentionColor;

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
            node.color = offColor;

        //set current mana
        protoMana = MyMana.GetComponent<ProtoMana>();
        int batteryCount = protoMana.CurrentBattery;
        int retentionCount = protoMana.CurrentRetention;

        for (int i = 0; i < maxBattery; i++)
        {
            if(i < retentionCount)
                batteryPips[i].color = retentionColor;
            else if (i < batteryCount)
                batteryPips[i].color = onColor;
            else
                batteryPips[i].color = offColor;
        }
    }
}
