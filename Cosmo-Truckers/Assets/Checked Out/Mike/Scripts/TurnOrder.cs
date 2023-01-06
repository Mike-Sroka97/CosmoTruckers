using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    List<CharacterSpeed> speedList;
    CharacterSpeed[] livingCharacters;
    private void Start()
    {
        speedList = new List<CharacterSpeed>();
        livingCharacters = FindObjectsOfType<CharacterSpeed>();
        foreach(CharacterSpeed speed in livingCharacters)
        {
            speedList.Add(speed);
        }
        DetermineTurnOrder();
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
}
