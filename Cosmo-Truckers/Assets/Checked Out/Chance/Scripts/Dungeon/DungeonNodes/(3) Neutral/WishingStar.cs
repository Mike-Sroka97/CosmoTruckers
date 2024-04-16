using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WishingStar : NCNodePopUpOptions
{
    public string dilemmaHeader = "Will you make a wish??";
    public Vector2Int percentAmounts = new Vector2Int(10, 25);
    int[] combos = new int[] { 0, 1, 2, 3, 4 };
    Dictionary<int, string> comboNames = new Dictionary<int, string>();
    int[] percentage;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }


    void OnButtonClick(int loc)
    {
        if(loc == 0)
        {
            allPlayersSorted[0].UpdateStat(combos[0], percentage[0]);
            allPlayersSorted[0].UpdateStat(combos[1], -percentage[1]);
        }
        else if(loc == 1)
        {
            allPlayersSorted[0].UpdateStat(combos[2], percentage[2]);
            allPlayersSorted[0].UpdateStat(combos[3], -percentage[3]);
        }

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        comboNames.Add(0, "Defence");
        comboNames.Add(1, "Vigor");
        comboNames.Add(2, "Speed");
        comboNames.Add(3, "Damage");
        comboNames.Add(4, "Restoration");

        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        MathCC.Shuffle(combos);
        percentage = new int[4] 
        {
            Random.Range(percentAmounts.x, percentAmounts.y + 1), 
            Random.Range(percentAmounts.x, percentAmounts.y + 1),
            Random.Range(percentAmounts.x, percentAmounts.y + 1),
            Random.Range(percentAmounts.x, percentAmounts.y + 1) 
        };
        base.SetUp(augs);

        GameObject firstOption = Instantiate(buttonToAdd, buttonLocation.transform);
        firstOption.GetComponentInChildren<TMP_Text>().text = $"+{percentage[0]}% {comboNames[combos[0]]}\n" +
                                                              $"-{percentage[1]}% {comboNames[combos[1]]}";

        firstOption.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(0); });

        firstOption.transform.localScale = Vector3.one;

        GameObject secondOption = Instantiate(buttonToAdd, buttonLocation.transform);
        secondOption.GetComponentInChildren<TMP_Text>().text = $"+{percentage[2]}% {comboNames[combos[2]]}\n" +
                                                               $"-{percentage[3]}% {comboNames[combos[3]]}";

        secondOption.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(1); });

        secondOption.transform.localScale = Vector3.one;


        GameObject noButton = Instantiate(buttonToAdd, buttonLocation.transform);
        noButton.GetComponentInChildren<TMP_Text>().text = $"Decline";
        noButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(5); });

        noButton.transform.localScale = Vector3.one;
    }
}
