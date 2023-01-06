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
        speed += speedMod;
        turnOrder.AdjustSpeed(this);
        turnOrder.DetermineTurnOrder();
    }

    public void SetSpeed(int newSpeed) { speed = newSpeed; }
    public int GetSpeed() { return speed; }
}
