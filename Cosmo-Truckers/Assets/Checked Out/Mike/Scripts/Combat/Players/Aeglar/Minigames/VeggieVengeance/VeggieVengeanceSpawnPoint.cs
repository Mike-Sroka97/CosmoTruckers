using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceSpawnPoint : MonoBehaviour
{
    [SerializeField] Vector2 xVelocity;
    Vector2 initialUpForce; 

    private void Awake()
    {
        initialUpForce = FindObjectOfType<VeggieVengeanceSpawner>().GetInitialUpForce(); 
    }

    public float GetInitialUpForce()
    {
        return (float)Math.Round(UnityEngine.Random.Range(initialUpForce.x, initialUpForce.y), 1); 
    }

    public float GetXVelocity()
    {
        return (float)Math.Round(UnityEngine.Random.Range(xVelocity.x, xVelocity.y), 1);
    }
}
