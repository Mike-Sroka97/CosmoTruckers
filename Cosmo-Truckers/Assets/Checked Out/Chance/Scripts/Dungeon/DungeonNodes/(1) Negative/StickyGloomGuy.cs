using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StickyGloomGuy : NCNodePopUpOptions
{
    [SerializeField] int HealthDeduction = 20;

    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        GameObject buttonAUG = Instantiate(buttonToAdd);
        buttonAUG.transform.SetParent(buttonLocation.transform);

        buttonAUG.GetComponentInChildren<TMP_Text>().text = augs[0].DebuffName;
        buttonAUG.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[0]); });

        GameObject buttonHP = Instantiate(buttonToAdd);
        buttonHP.transform.SetParent(buttonLocation.transform);

        buttonHP.GetComponentInChildren<TMP_Text>().text = $"Just a little damage\n-{HealthDeduction}";
        buttonHP.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(); });
    }

    void OnButtonClick()
    {
        allPlayersSorted[0].TakeDamage(HealthDeduction);

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }
}
