using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeed : MonoBehaviour
{
    [SerializeField] public int speed;

    public void SetSpeed(int newSpeed) { speed = newSpeed; }
    public int GetSpeed() { return speed; }
}
