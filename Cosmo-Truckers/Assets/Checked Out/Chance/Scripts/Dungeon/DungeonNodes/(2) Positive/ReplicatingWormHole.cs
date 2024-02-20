using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplicatingWormHole : NCNodePopUpOptions
{
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        GameObject buttonDouble = Instantiate(buttonToAdd, buttonLocation.transform);

        buttonDouble.GetComponentInChildren<TMP_Text>().text = "Double Random AUG";
        buttonDouble.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(true); });

        buttonDouble.transform.localScale = Vector3.one;

        GameObject buttonNo = Instantiate(buttonToAdd, buttonLocation.transform);

        buttonNo.GetComponentInChildren<TMP_Text>().text = "No Thanks";
        buttonNo.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(false); });

        buttonNo.transform.localScale = Vector3.one;
    }

    public void OnButtonClick(bool yes)
    {
        if(yes)
            allPlayersSorted[0].DoubleAugment(1);

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }
}
