using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bribery : MonoBehaviour
{
    [SerializeField] float[] startDelays;
    [SerializeField] float moneySpawnDelay;
    [SerializeField] GameObject[] rows;
    [HideInInspector] public int Score;
    [HideInInspector] public bool[] ActivatedRows;

    BriberyEnemy[] enemies;
    BriberyCollectable[] collectables;
    float currentTime = 0;
    bool allFull = false;

    private void Awake()
    {
        enemies = GetComponentsInChildren<BriberyEnemy>();
        List<int> randomIndices = new List<int>(new int[enemies.Length]);

        int random = 0;
        bool first = true;

        for(int i = 0; i < enemies.Length; i++)
        {
            if(first)
            {
                random = UnityEngine.Random.Range(1, enemies.Length + 1);
                randomIndices[i] = random;
                first = false;
            }
            else
            {
                while (randomIndices.Contains(random))
                {
                    random = UnityEngine.Random.Range(1, enemies.Length + 1);
                }
                randomIndices[i] = random;
            }
        }

        for(int i = 0; i < randomIndices.Count; i++)
        {
            randomIndices[i]--;
        }

        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].StartDelay = startDelays[randomIndices[i]];
        }
    }

    private void Start()
    {
        collectables = GetComponentsInChildren<BriberyCollectable>();
        ActivatedRows = new bool[rows.Length];
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= moneySpawnDelay)
        {
            currentTime = 0;
            SpawnMoney();
        }
    }

    private void SpawnMoney()
    {
        foreach(bool row in ActivatedRows)
        {
            allFull = true;
            if (row == false)
            {
                allFull = false;
                break;
            }
        }

        if(!allFull)
        {
            int random = UnityEngine.Random.Range(0, ActivatedRows.Length);
            while (ActivatedRows[random] == true)
            {
                random = UnityEngine.Random.Range(0, ActivatedRows.Length);
            }

            int random2 = UnityEngine.Random.Range(0, 3); //three per row

            rows[random].GetComponentsInChildren<BriberyCollectable>()[random2].Activate();
        }
    }
}
