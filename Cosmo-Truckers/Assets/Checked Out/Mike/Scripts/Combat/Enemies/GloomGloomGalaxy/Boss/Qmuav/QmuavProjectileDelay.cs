using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavProjectileDelay : MonoBehaviour
{
    [SerializeField] GameObject[] projectilesWaves;
    [SerializeField] float timeBetweenWaves;

    int currentIndex = 0;

    public IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        projectilesWaves[currentIndex].SetActive(true);
        currentIndex++;

        if(currentIndex < projectilesWaves.Length)
        {
            StartCoroutine(SpawnWave());
        }
    }
}
