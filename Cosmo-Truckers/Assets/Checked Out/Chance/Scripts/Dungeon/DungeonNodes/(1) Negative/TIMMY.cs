using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TIMMY : NCNodePopUpOptions
{
    [SerializeField] GameObject Timmy;

    public override void SetUp(DebuffStackSO[] augs)
    {
        CombatData.Instance.EnemySummonsToSpawn.Add(Timmy);

        currentPlayer.text = "Would you like to hear a joke?";

        GameObject button = Instantiate(buttonToAdd);
        button.transform.SetParent(buttonLocation.transform);

        button.GetComponentInChildren<TMP_Text>().text = "NO";

        button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(); Destroy(button); });

        button.transform.localScale = Vector3.one;
    }

    void OnButtonClick()
    {
        StartCoroutine(ShowResponce());
    }

    IEnumerator ShowResponce()
    {
        currentPlayer.text = "Well what a shame then. . .";

        yield return new WaitForSeconds(1.0f);

        Destroy(this.gameObject);
    }
}
