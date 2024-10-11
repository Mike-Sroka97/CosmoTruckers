using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SproutingStar : EventNodeBase
{
    [SerializeField] int burnDamage = 10;
    [SerializeField] int baseChance = 10;
    int percentChance;

    protected override void Start()
    {
        percentChance = baseChance;
        base.Start();

        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"Touch<color=green> [Cleanse (1)]</color>\n<color=red> [{percentChance}% chance to take (10) damage]</color>";
    }

    public override void Initialize(EventNodeHandler handler)
    {
        base.Initialize(handler);
        CheckFirstButtonEnable();
    }

    public void Cleanse()
    {
        int random = Random.Range(1, 101); //1-100

        CheckFirstButtonEnable();

        if (random <= percentChance)
            currentCharacter.TakeDamage(10, true);

        //cleanse 1
        currentCharacter.RemoveAmountOfAugments(1, 0);
        
        percentChance += baseChance;

        if (percentChance > 100)
            percentChance = 100;  

        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"Touch<color=green> [Cleanse (1)]</color>\n<color=red> [{percentChance}% chance to take (10) damage]</color>";
    }

    private void CheckFirstButtonEnable()
    {
        if (currentCharacter.CurrentHealth - burnDamage <= burnDamage)
        {
            myButtons[0].enabled = false;
            AutoSelectNextButton();
        }
    }

    public override void IgnoreOption()
    {
        percentChance = baseChance;
        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"Touch<color=green> [Cleanse (1)]</color>\n<color=red> [{percentChance}% chance to take (10) damage]</color>";
        myButtons[0].enabled = true;
        base.IgnoreOption();
    }
}
