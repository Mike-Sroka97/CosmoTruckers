using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Speed;

    TurnOrder turnOrder;

    private void Start()
    {
        turnOrder = FindObjectOfType<TurnOrder>();
    }

    public void SpeedChange(int speedMod)
    {
        bool turnOrderAdjusted;
        int tempSpeed = Speed;
        Speed += speedMod;
        if(tempSpeed > Speed)
        {
            turnOrderAdjusted = turnOrder.AdjustSpeed(this, false);
        }
        else
        {
            turnOrderAdjusted = turnOrder.AdjustSpeed(this, true);
        }
        if(turnOrderAdjusted)
        {
            turnOrder.DetermineTurnOrder();
        }
        else
        {
            Speed = tempSpeed;
        }
    }
}
