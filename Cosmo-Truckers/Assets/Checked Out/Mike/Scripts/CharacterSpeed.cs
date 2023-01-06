using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeed : MonoBehaviour
{
    [SerializeField] public int speed;

    TurnOrder turnOrder;

    private void Start()
    {
        turnOrder = FindObjectOfType<TurnOrder>();
    }

    public void SpeedChange(int speedMod)
    {
        bool turnOrderAdjusted;
        int tempSpeed = speed;
        speed += speedMod;
        if(tempSpeed > speed)
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
            speed = tempSpeed;
        }
    }

    public void SetSpeed(int newSpeed) { speed = newSpeed; }
    public int GetSpeed() { return speed; }
}
