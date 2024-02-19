using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AstroDog : NCNodePopUpOptions
{
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        for(int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            string buttonText = "";
            switch(i)
            {
                case 0:
                    buttonText = "Pet the Dog";
                    break;
                case 1:
                    buttonText = "Play with the Dog";
                    break;
                case 2:
                    buttonText = "Shake the Dog's paw";
                    break;
                case 3:
                    buttonText = "Feed the dog";
                    break;

                default: break;
            }

            button.GetComponentInChildren<TMP_Text>().text = buttonText;
            int choice = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(choice); Destroy(button); });

            button.transform.localScale = Vector3.one;
        }
    }

    public void OnButtonClick(int loc)
    {
        switch(loc)
        {
            case 0:
                allPlayersSorted[0].ProliferateAugment(1, 1);
                break;
            case 1:
                allPlayersSorted[0].RemoveAmountOfAugments(1, 0);
                break;
            case 2:
                allPlayersSorted[0].AdjustDamage(10);
                break;
            case 3:
                allPlayersSorted[0].TakeHealing(5);
                break;

            default: break;
        }

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }
}
