using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SproutingStar : NCNodePopUpOptions
{
    int damageChance = 5;
    int amountOfDamage = 10;

    int currentChance = 15;

    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        GameObject leftButton = Instantiate(buttonToAdd, buttonLocation.transform);
        leftButton.GetComponentInChildren<TMP_Text>().text = $"Remove a debuff";
        leftButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(0); });

        leftButton.transform.localScale = Vector3.one;


        GameObject rightButton = Instantiate(buttonToAdd, buttonLocation.transform);
        rightButton.GetComponentInChildren<TMP_Text>().text = $"I'm done";
        rightButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(1); });

        rightButton.transform.localScale = Vector3.one;
    }

    void OnButtonClick(int loc)
    {
        if (loc == 1)
        {
            allPlayersSorted.RemoveAt(0);

            if (allPlayersSorted.Count > 0)
            {
                currentChance = 15;
                ShowPlayerName(allPlayersSorted[0].CharacterName);
            }
            else
                Destroy(this.gameObject);
        }
        else
        {
            int damage = Random.Range(0, 101);
            if (damage < currentChance)
                allPlayersSorted[0].TakeDamage(amountOfDamage, true);

            currentChance += damageChance;

            allPlayersSorted[0].RemoveAmountOfAugments(1, 0);
        }
    }
}
