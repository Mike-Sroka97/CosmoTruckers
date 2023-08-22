using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavProjectileDelay : MonoBehaviour
{
    [SerializeField] GameObject[] projectilesWaves;
    [SerializeField] float timeBetweenWaves;

    int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
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
