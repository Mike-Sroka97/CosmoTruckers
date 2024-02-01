using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Reflex = 1;
    public int Defense = 0;
    public int Vigor = 100;
    public int Speed = 100;
    public int Damage = 100;
    public int Restoration = 100;
    public float Gravity = 1;

    TurnOrder turnOrder;

    private void Start()
    {
        turnOrder = FindObjectOfType<TurnOrder>();
    }

    public void ReflexChange(int speedMod)
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
