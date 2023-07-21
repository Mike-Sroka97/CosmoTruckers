using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidShlurpingSalivaSpawn : MonoBehaviour
{
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;
    [SerializeField] GameObject bullet;

    float currentTime = 0;
    float currentSpawnDelay;

    private void Start()
    {
        CalculateSpawnDelay();
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= currentSpawnDelay)
        {
            currentTime = 0;
            CalculateSpawnDelay();
            GameObject tempBullet = Instantiate(bullet, transform);
            tempBullet.transform.parent = null;
        }
    }

    private void CalculateSpawnDelay()
    {
        currentSpawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}
