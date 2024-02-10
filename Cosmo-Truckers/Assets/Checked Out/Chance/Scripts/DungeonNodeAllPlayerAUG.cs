using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonNodeAllPlayerAUG : MonoBehaviour
{
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] GameObject buttonLocation;
    [SerializeField] GameObject buttonToAdd;

    List<PlayerCharacter> allPlayersSorted = new();

    public void SetUpPlayerOptions(DebuffStackSO[] augs)
    {
        int augIndex = 0;

        foreach(var player in EnemyManager.Instance.Players)
        {
            allPlayersSorted.Add(player);
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            button.GetComponentInChildren<TMP_Text>().text = augs[augIndex].DebuffName + " " + augIndex;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[augIndex]); Destroy(button); });

            augIndex = augIndex + 1 > augs.Length - 1 ? 0 : augIndex + 1;
        }

        //Sort players by reflex stat
        allPlayersSorted = allPlayersSorted.OrderBy(x => x.Stats.Reflex).ToList();

        ShowPlayerName();
    }

    void ShowPlayerName()
    {
        currentPlayer.text = $"{allPlayersSorted[0].CharacterName}'s choice";
    }

    void OnButtonClick(DebuffStackSO augToAdd)
    {
        DebuffStackSO stackToAdd = Instantiate(augToAdd);
        bool added = false;

        foreach (DebuffStackSO aug in allPlayersSorted[0].GetAUGS)
        {
            if (string.Equals(aug.DebuffName, stackToAdd.DebuffName))
            {
                if (aug.Stackable && aug.CurrentStacks < aug.MaxStacks)
                {
                    aug.CurrentStacks += 1;
                    Debug.Log($"{allPlayersSorted[0].CharacterName} added stack of {stackToAdd.DebuffName}");
                }
                else
                {
                    Debug.Log($"{allPlayersSorted[0].CharacterName} has max stacks of {stackToAdd.DebuffName}");
                }

                added = true;
                break;
            }
        }

        if (!added)
        {
            allPlayersSorted[0].GetAUGS.Add(stackToAdd);
            Debug.Log($"{allPlayersSorted[0].CharacterName} has been given {stackToAdd.DebuffName}");
        }

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName();
        else
            Destroy(this.gameObject);
    }
}
