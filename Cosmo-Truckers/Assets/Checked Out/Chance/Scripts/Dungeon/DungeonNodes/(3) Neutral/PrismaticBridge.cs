using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrismaticBridge : NCNodePopUpOptions
{
    public string header = "Make your choice. . .";
    public int damage = 15;
    public int healing = 20;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc)
    {
        switch (loc)
        {
            case 0:
                allPlayersSorted[0].RemoveAmountOfAugments(999, 1);
                break;
            case 1:
                allPlayersSorted[0].GetManaBase.SetMaxMana();
                break;
            case 2:
                allPlayersSorted[0].TakeDamage(damage);
                break;
            case 3:
                allPlayersSorted[0].TakeHealing(healing);
                break;
            default: break;
        }

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        for (int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            switch (i)
            {
                case 0:
                    button.GetComponentInChildren<TMP_Text>().text = "Lose all buffs";
                    break;
                case 1:
                    button.GetComponentInChildren<TMP_Text>().text = "Full mana";
                    break;
                case 2:
                    button.GetComponentInChildren<TMP_Text>().text = $"Take {damage} damage";
                    break;
                case 3:
                    button.GetComponentInChildren<TMP_Text>().text = $"Heal {healing} damage";
                    break;
                default: break;
            }

            button.transform.localScale = Vector3.one;
            int loc = i;

            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(i); button.gameObject.SetActive(false); });
        }
    }
}
