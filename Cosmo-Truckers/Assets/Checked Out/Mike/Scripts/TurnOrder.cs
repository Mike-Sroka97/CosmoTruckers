using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    CharacterSpeed[] livingCharacters;
    private void Start()
    {
        DetermineTurnOrder();
    }

    public void DetermineTurnOrder()
    {
        livingCharacters = FindObjectsOfType<CharacterSpeed>();
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
}
