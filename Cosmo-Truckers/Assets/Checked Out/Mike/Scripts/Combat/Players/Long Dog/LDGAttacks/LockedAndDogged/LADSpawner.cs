using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LADSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBetweenProjectiles;
    [SerializeField] int numberOfProjectilesToSpawn;
    [SerializeField] GameObject projectile;
    [SerializeField] bool offsetMe;

    float currentTime = 0;
    int lastIndex = -1;
    float offset;
    int currentSpawned = 0;

    private void Start()
    {
        if(offsetMe)
        {
            offset = timeBetweenProjectiles / 2;
            currentTime -= offset;
        }

    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if(currentSpawned < numberOfProjectilesToSpawn)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeBetweenProjectiles)
            {
                currentTime = 0;
                RandomPosition();
            }
        }
    }

    private void RandomPosition()
    {
        int currentIndex = lastIndex;
        if(lastIndex == -1)
        {
            currentIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            lastIndex = currentIndex;
        }
        else
        {
            while(currentIndex == lastIndex)
            {
                currentIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            }
            lastIndex = currentIndex;
        }
        Instantiate(projectile, spawnPoints[currentIndex]);
        currentSpawned++;
    }
}
