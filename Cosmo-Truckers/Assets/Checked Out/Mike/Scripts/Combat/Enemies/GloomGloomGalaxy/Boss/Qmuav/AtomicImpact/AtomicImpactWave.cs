using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpactWave : MonoBehaviour
{
    AtomicImpact minigame;
    [SerializeField] GameObject[] waves; 
    [SerializeField] float newWaveWaitTime = 5f;

    private float waveTime = 0f; 
    private int waveCount = 0;

    private void Start()
    {
        minigame = FindObjectOfType<AtomicImpact>();

        for (int i = 1; i < waves.Length; i++)
            waves[i].SetActive(false);
    }

    private void Update()
    {
        if (waveCount < (waves.Length - 1))
        {
            waveTime += Time.deltaTime;

            if (waveTime > newWaveWaitTime)
            {
                waveTime = 0;
                waves[waveCount].SetActive(false);
                waveCount++; 
                waves[waveCount].SetActive(true);
            }
        }
    }
}
