using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NCNodePopUpOptions : MonoBehaviour
{
    [SerializeField] protected TMP_Text currentPlayer;
    [SerializeField] protected GameObject buttonLocation;
    [SerializeField] protected GameObject buttonToAdd;

    protected List<PlayerCharacter> allPlayersSorted = new();

    public virtual void SetUp(DebuffStackSO[] augs)
    {
        //int augIndex = 0;

        foreach(var player in EnemyManager.Instance.Players)
        {
            allPlayersSorted.Add(player);
            //GameObject button = Instantiate(buttonToAdd);
            //button.transform.SetParent(buttonLocation.transform);

            //button.GetComponentInChildren<TMP_Text>().text = augs[augIndex].DebuffName + " " + augIndex;
            //button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[augIndex]); Destroy(button); });

            //augIndex = augIndex + 1 > augs.Length - 1 ? 0 : augIndex + 1;

            //button.transform.scale = Vector3.one;
        }

        //Sort players by reflex stat
        allPlayersSorted = allPlayersSorted.OrderBy(x => x.Stats.Reflex).ToList();

        ShowPlayerName(allPlayersSorted[0].CharacterName);
    }

    /// <summary>
    /// Will add AUG to current player and go to next player
    /// </summary>
    /// <param name="augToAdd">The AUG to add</param>
    protected virtual void OnButtonClick(DebuffStackSO augToAdd)
    {

        allPlayersSorted[0].AddDebuffStack(augToAdd);
        Debug.Log($"{allPlayersSorted[0].CharacterName} has been given {augToAdd.DebuffName}");

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    protected void ShowPlayerName(string playerName)
    {
        currentPlayer.text = $"{playerName}'s choice";
    }
}
