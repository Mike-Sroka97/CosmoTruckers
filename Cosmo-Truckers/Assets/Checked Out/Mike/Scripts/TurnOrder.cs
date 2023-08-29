using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    [SerializeField] GameObject lootPopUp;

    List<CharacterStats> speedList;
    CharacterStats[] livingCharacters;
    int currentCharactersTurn = 0;
    private void Start()
    {
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        //Small wait before combat starts
        yield return new WaitForSeconds(1.5f);

        speedList = new List<CharacterStats>();
        livingCharacters = FindObjectsOfType<CharacterStats>();
        foreach (CharacterStats speed in livingCharacters)
        {
            speedList.Add(speed);
        }

        DetermineTurnOrder();
        StartTurn();
    }

    public void DetermineTurnOrder()
    {        
        Array.Sort(livingCharacters, new SpeedComparer());

        foreach (CharacterStats speed in livingCharacters)
        {
            if (speed.GetComponent<PlayerCharacter>())
            {
                //Debug.Log(speed.GetComponent<PlayerCharacter>().GetName() + " " + speed.speed);
            }
            else
            {
                //Debug.Log(speed.GetComponent<Enemy>().GetName() + " " + speed.speed);
            }
        }
    }

    private void StartTurn()
    {
        //give player control if player
        //give ai control if AI
        if(livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
        {
            livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().StartTurn();
        }
        else
        {
            livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
        }

        Debug.Log("It is " + livingCharacters[currentCharactersTurn].name + "'s turn");
    }

    public void EndTurn()
    {
        if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
        {
            livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().EndTurn();
        }
        else
        {
            livingCharacters[currentCharactersTurn].GetComponent<Enemy>().EndTurn();
        }

        currentCharactersTurn++;
        if(currentCharactersTurn >= livingCharacters.Length)
        {
            currentCharactersTurn = 0;
        }
        StartTurn();
    }

    public bool AdjustSpeed(CharacterStats characterSpeed, bool increase)
    {
        //prevents the skipping of a characters turn
        if(increase)
        {
            if (currentCharactersTurn + 1 < livingCharacters.Length)
            {
                if (livingCharacters[currentCharactersTurn + 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
            else
            {
                if (livingCharacters[0].name == characterSpeed.name)
                {
                    return false;
                }
            }
        }
        else
        {
            if (currentCharactersTurn - 1 > 0)
            {
                if (livingCharacters[currentCharactersTurn - 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
            else
            {
                if (livingCharacters[livingCharacters.Length - 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
        }

        foreach (CharacterStats speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                Debug.Log("here");
                speed.Speed = characterSpeed.Speed;
            }
        }
        return true;
    }

    public void RemoveFromSpeedList(CharacterStats characterSpeed)
    {
        foreach(CharacterStats speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                speedList.Remove(speed);
                break;
            }
        }
        livingCharacters = speedList.ToArray();
    }

    public void AddToSpeedList(CharacterStats characterSpeed)
    {
        foreach (CharacterStats speed in speedList)
        {
            if (speed.gameObject.name == characterSpeed.name)
            {
                return;
            }
        }
        speedList.Add(characterSpeed);
        livingCharacters = speedList.ToArray();
    }

    public void EndCombat()
    {
        lootPopUp.SetActive(true);
        LootSlot[] slots = lootPopUp.GetComponentsInChildren<LootSlot>();
        List<Loot> thisCombatsLoot = FindObjectOfType<LootManager>().GetLoot();
        if(thisCombatsLoot.Count > 0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].ToggleImage(true);
                slots[i].SetImage(thisCombatsLoot[i].GetImage());
                thisCombatsLoot[i].SetName(thisCombatsLoot[i].GetName() + thisCombatsLoot[i].GetLootCount());
                slots[i].SetMyText(thisCombatsLoot[i].GetName());
                if (i + 1 >= thisCombatsLoot.Count)
                {
                    break;
                }
            }
        }
    }
}
