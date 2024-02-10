using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmicCandyBowl : NCNodePopUpOptions
{
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        List<DebuffStackSO> augsToGet = new List<DebuffStackSO>(augs.ToList());
        int augIndex = Random.Range(0, augsToGet.Count);

        foreach (var player in allPlayersSorted)
        {
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            button.GetComponentInChildren<TMP_Text>().text = augsToGet[augIndex].DebuffName;
            DebuffStackSO stackToGive = augsToGet[augIndex];
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(stackToGive); Destroy(button); });

            augsToGet.RemoveAt(augIndex);

            augIndex = Random.Range(0, augsToGet.Count);

            button.transform.localScale = Vector3.one;
        }
    }
}
