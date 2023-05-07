using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchySpineSpawner : MonoBehaviour
{
    [SerializeField] GameObject goodProjectile;
    [SerializeField] GameObject badProjectile;
    [SerializeField] float projectileSpawnRate;
    [SerializeField] float spawnRotationVariance;

    const int maxGoodProjectiles = 5;
    const int maxBadProjectiles = 10;
    int currentGoodProjectiles = 0;
    int currentBadProjectiles = 0;
    int tempType = -1;
    int random;
    float currentTime = 0;
    float tempAngle;

    private void Update()
    {
        TrackTime();
        currentTime += Time.deltaTime;
    }

    private void TrackTime()
    {
        if (currentTime >= projectileSpawnRate)
        {
            currentTime = 0;
            if(tempType == 1)
            {
                random = 0;
            }
            else
            {
                random = Random.Range(0, 2);
            }

            tempType = random;
            float randomAngle = Random.Range(-spawnRotationVariance, spawnRotationVariance);
            if((randomAngle < 0 && randomAngle < 0) || (randomAngle > 0 && randomAngle > 0))
            {
                randomAngle = -randomAngle;
            }
            tempAngle = randomAngle;
            Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
            if (random == 1)
            {
                if (currentGoodProjectiles < maxGoodProjectiles)
                {
                    currentGoodProjectiles++;
                    Instantiate(goodProjectile, transform.position, rotation);
                }
                else if (currentBadProjectiles < maxBadProjectiles)
                {
                    currentBadProjectiles++;
                    Instantiate(badProjectile, transform.position, rotation);
                }
            }
            else
            {
                if (currentBadProjectiles < maxBadProjectiles)
                {
                    currentBadProjectiles++;
                    Instantiate(badProjectile, transform.position, rotation);
                }
                else if (currentGoodProjectiles < maxGoodProjectiles)
                {
                    currentGoodProjectiles++;
                    Instantiate(goodProjectile, transform.position, rotation);
                }
            }
        }
    }
}
