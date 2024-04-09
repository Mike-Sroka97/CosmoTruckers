using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShakeTheStarlightSoda : NCNodePopUpOptions
{
    public string dilemmaHeader = "Shake the soda and test your luck??";
    public int popDamage = 18;
    public int playerShakesAvalible = 3;
    public Vector2Int shakeRange = new Vector2Int(3, 7);

    int totalShakes = 0;
    int shakesToPop;
    int playerShakes = 0;


    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }


    protected override void OnButtonClick(DebuffStackSO augToAdd)
    {
        playerShakes++;
        totalShakes++;

        allPlayersSorted[0].AddDebuffStack(augToAdd);

        //The soda has poped
        if(totalShakes == shakesToPop)
        {
            allPlayersSorted[0].TakeDamage(popDamage);

            Destroy(this.gameObject);
        }
        //Took all shakes avalible
        //Skipping this player
        else if(playerShakes == playerShakesAvalible)
        {
            NoButtonClick();
        }
        
    }

    void NoButtonClick()
    {
        Debug.Log($"{allPlayersSorted[0].CharacterName} passed the soda");

        playerShakes = 0;
        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        shakesToPop = Random.Range(shakeRange.x, shakeRange.y);

        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject yesButton = Instantiate(buttonToAdd, buttonLocation.transform);
        yesButton.GetComponentInChildren<TMP_Text>().text = $"Shake the soda";
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[0]); });

        yesButton.transform.localScale = Vector3.one;


        GameObject noButton = Instantiate(buttonToAdd, buttonLocation.transform);
        noButton.GetComponentInChildren<TMP_Text>().text = $"Hand it off";
        noButton.GetComponent<Button>().onClick.AddListener(delegate { NoButtonClick(); });

        noButton.transform.localScale = Vector3.one;
    }
}
