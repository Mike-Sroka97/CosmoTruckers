using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    List<CharacterSpeed> speedList;
    CharacterSpeed[] livingCharacters;
    int currentCharactersTurn = 0;
    private void Start()
    {
        speedList = new List<CharacterSpeed>();
        livingCharacters = FindObjectsOfType<CharacterSpeed>();
        foreach(CharacterSpeed speed in livingCharacters)
        {
            speedList.Add(speed);
        }

        DetermineTurnOrder();
        StartTurn();
    }

    public void DetermineTurnOrder()
    {        
        Array.Sort(livingCharacters, new SpeedComparer());

        foreach (CharacterSpeed speed in livingCharacters)
        {
            if (speed.GetComponent<PlayerCharacter>())
            {
                Debug.Log(speed.GetComponent<PlayerCharacter>().GetName() + " " + speed.speed);
            }
            else
            {
                Debug.Log(speed.GetComponent<Enemy>().GetName() + " " + speed.speed);
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

        currentCharactersTurn++;
        if(currentCharactersTurn >= livingCharacters.Length)
        {
            currentCharactersTurn = 0;
        }
        StartTurn();
    }

    public bool AdjustSpeed(CharacterSpeed characterSpeed, bool increase)
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

        foreach (CharacterSpeed speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                Debug.Log("here");
                speed.speed = characterSpeed.speed;
            }
        }
        return true;
    }

    public void RemoveFromSpeedList(CharacterSpeed characterSpeed)
    {
        foreach(CharacterSpeed speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                speedList.Remove(speed);
                break;
            }
        }
        livingCharacters = speedList.ToArray();
    }

    public void AddToSpeedList(CharacterSpeed characterSpeed)
    {
        foreach (CharacterSpeed speed in speedList)
        {
            if (speed.gameObject.name == characterSpeed.name)
            {
                return;
            }
        }
        speedList.Add(characterSpeed);
        livingCharacters = speedList.ToArray();
    }
}
