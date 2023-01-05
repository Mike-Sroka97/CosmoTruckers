using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeed : MonoBehaviour
{
    [SerializeField] public int speed;

    private void Start()
    {
        
    }

    private void SpeedChange(int newSpeed)
    {
        speed = newSpeed;
        FindObjectOfType<TurnOrder>().DetermineTurnOrder();
    }

    public void SetSpeed(int newSpeed) { speed = newSpeed; }
    public int GetSpeed() { return speed; }
}
