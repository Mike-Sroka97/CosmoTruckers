using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCrustStalatiteSpawner : MonoBehaviour
{
    [SerializeField] GameObject stalactite;
    [SerializeField] int maxNumberOfSpawns;
    [HideInInspector] public bool IsFull;

    int currentNumberOfSpawns = 0;

    public void SpawnStalactite()
    {
        currentNumberOfSpawns++;
        Instantiate(stalactite, transform);

        if(currentNumberOfSpawns >= maxNumberOfSpawns)
        {
            IsFull = true;
        }
    }
}
