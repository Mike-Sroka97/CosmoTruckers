using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int Reflex = 1;
    public int Defense = 0;
    [HideInInspector] public int TrueDefense;
    public int Vigor = 100;
    [HideInInspector] public int TrueVigor;
    public int Speed = 100;
    [HideInInspector] public int TrueSpeed;
    public int Damage = 100;
    [HideInInspector] public int TrueDamage;
    public int Restoration = 100;
    [HideInInspector] public int TrueRestoration;
    public float Gravity = 1;
    [HideInInspector] public float TrueGravity;

    private void Start()
    {
        TrueDefense = Defense;
        TrueVigor = Vigor;
        TrueDamage = Damage;
        TrueRestoration = Restoration;
        TrueGravity = Gravity;
    }

    public void ReflexChange(int speedMod)
    {
        bool turnOrderAdjusted;
        int tempSpeed = Reflex;
        Reflex += speedMod;
        if(tempSpeed > Reflex)
        {
            turnOrderAdjusted = TurnOrder.Instance.AdjustSpeed(this, false);
        }
        else
        {
            turnOrderAdjusted = TurnOrder.Instance.AdjustSpeed(this, true);
        }
        if(turnOrderAdjusted)
        {
            TurnOrder.Instance.DetermineTurnOrder();
        }
        else
        {
            Reflex = tempSpeed;
        }
    }
}
