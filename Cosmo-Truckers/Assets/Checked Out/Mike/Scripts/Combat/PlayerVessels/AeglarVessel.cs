using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AeglarVessel : PlayerVessel
{
    [SerializeField] TextMeshProUGUI[] manaTypeTexts;

    AeglarMana aeglarMana;

    const int veggieIndex = 0;
    const int meatIndex = 1;
    const int sweetIndex = 2;
    public override void ManaTracking()
    {
        //set current mana
        aeglarMana = MyMana.GetComponent<AeglarMana>();

        manaTypeTexts[veggieIndex].text = aeglarMana.VeggieMana.ToString();
        manaTypeTexts[meatIndex].text = aeglarMana.MeatMana.ToString();
        manaTypeTexts[sweetIndex].text = aeglarMana.SweetMana.ToString();
    }
}
