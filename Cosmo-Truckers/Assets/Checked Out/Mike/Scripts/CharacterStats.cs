using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Reflex;
    public int Defense;
    public int Vigor;
    public int Speed;

    TurnOrder turnOrder;

    private void Start()
    {
        turnOrder = FindObjectOfType<TurnOrder>();
    }

    public void SpeedChange(int speedMod)
    {
        bool turnOrderAdjusted;
        int tempSpeed = Reflex;
        Reflex += speedMod;
        if(tempSpeed > Reflex)
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
            Reflex = tempSpeed;
        }
    }
}
