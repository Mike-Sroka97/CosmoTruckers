using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeTVessel : PlayerVessel
{
    [SerializeField] Image[] angerNodes;
    [SerializeField] Color offColor;
    [SerializeField] Color angerColor;
    [SerializeField] Color rageColor;

    const int ragePip = 3;

    public override void ManaTracking()
    {
        //reset mana
        foreach (Image node in angerNodes)
            node.color = offColor;

        //set current mana
        SafeTMana safeTMana = MyMana.GetComponent<SafeTMana>();
        int totalAnger = safeTMana.GetCurrentAnger();
        int totalRage = safeTMana.GetCurrentRage();

        for (int i = 0; i < totalAnger; i++)
        {
            switch(totalRage)
            {
                case 0:
                    angerNodes[i].color = angerColor;
                    break;
                case 1:
                    if (i < 3)
                        angerNodes[i].color = rageColor;
                    else
                        angerNodes[i].color = angerColor;
                    break;
                case 2:
                    if (i < 6)
                        angerNodes[i].color = rageColor;
                    else
                        angerNodes[i].color = angerColor;
                    break;
                case 3:
                    angerNodes[i].color = rageColor;
                    break;
                default:
                    break;
            }
        }
    }
}
