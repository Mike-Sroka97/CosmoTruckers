using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KleptersKravester : NCNodePopUpOptions
{
    [SerializeField] Sprite[] AllFruitImages;
    [SerializeField] bool Possitive = false;

    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);
        int badChoice = Random.Range(0, allPlayersSorted.Count);

        for (int i = 0; i < allPlayersSorted.Count; i++)
        {
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);
            button.GetComponentInChildren<TMP_Text>().text = "";
            button.GetComponent<Image>().sprite = AllFruitImages[Random.Range(0, AllFruitImages.Length)];

            button.transform.localScale = Vector3.one;

            if(i == badChoice)
                button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClickBad(); button.gameObject.SetActive(false); });
            else
                button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(); button.gameObject.SetActive(false); });
        }
    }

    void OnButtonClick()
    {
        if(Possitive)
            allPlayersSorted[0].TakeHealing(1, true);
        else
            allPlayersSorted[0].TakeDamage(1, true);

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    void OnButtonClickBad()
    {
        if(Possitive)
            allPlayersSorted[0].Energize(true);
        else
            allPlayersSorted[0].Stun(true);

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }
}
