using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_AmberStarSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject amberStar;

    int lastIndex = 10;
    int spawnPos = 10;
    float currentTime = 0;
    float spawnTime;
    bool spawnStar = true;
    bool spawnTimeDecided = false;

    private void Update()
    {
        if(!spawnTimeDecided)
        {
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            spawnTimeDecided = true;
        }
        else if(currentTime < spawnTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            if (lastIndex == 10)
            {
                spawnPos = Random.Range(0, spawnPositions.Length);
                lastIndex = spawnPos;
                Instantiate(amberStar, spawnPositions[spawnPos]);
            }
            else
            {
                while(spawnPos == lastIndex)
                {
                    spawnPos = Random.Range(0, spawnPositions.Length);
                }
                lastIndex = spawnPos;
                Instantiate(amberStar, spawnPositions[spawnPos]);
            }
            spawnTimeDecided = false;
            currentTime = 0;
        }
    }
}
