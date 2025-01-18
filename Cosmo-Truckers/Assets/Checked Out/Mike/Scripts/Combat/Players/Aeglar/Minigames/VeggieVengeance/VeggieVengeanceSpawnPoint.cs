using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceSpawnPoint : MonoBehaviour
{
    Vector2 initialUpForce = new Vector2(4.6f, 5.21f);
    [SerializeField] Vector2 xVelocity;

    public float GetInitialUpForce()
    {
        return (float)Math.Round(UnityEngine.Random.Range(initialUpForce.x, initialUpForce.y), 2); 
    }

    public float GetXVelocity()
    {
        return (float)Math.Round(UnityEngine.Random.Range(xVelocity.x, xVelocity.y), 2);
    }
}
