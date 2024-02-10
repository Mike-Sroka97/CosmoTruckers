using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoisyNuissance : NCNodePopUpOptions
{
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        foreach(var player in allPlayersSorted)
        {
            player.AddDebuffStack(augs[0]);
        }

        GameObject button = Instantiate(buttonToAdd);
        button.transform.SetParent(buttonLocation.transform);

        button.GetComponentInChildren<TMP_Text>().text = "Take another stack and gain 5 HP";
        button.GetComponent<Button>().onClick.AddListener(delegate { allPlayersSorted[0].TakeHealing(5); OnButtonClick(augs[0]); });

        button.transform.localScale = Vector3.one;

        button = Instantiate(buttonToAdd);
        button.transform.SetParent(buttonLocation.transform);

        button.GetComponentInChildren<TMP_Text>().text = "No thanks";
        button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(); });

        button.transform.localScale = Vector3.one;
    }

    void OnButtonClick()
    {
        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }
}
