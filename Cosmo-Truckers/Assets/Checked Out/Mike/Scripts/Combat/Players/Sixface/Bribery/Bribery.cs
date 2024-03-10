using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bribery : CombatMove
{
    [SerializeField] DebuffStackSO bribery;
    [SerializeField] float[] startDelays;
    [SerializeField] float moneySpawnDelay;
    [SerializeField] float delayIncrement;
    [SerializeField] GameObject[] rows;
    [SerializeField] float[] movementSpeeds;
    [SerializeField] float[] sendBackDistances;
    [HideInInspector] public bool[] ActivatedRows;
    [HideInInspector] public bool[] DisabledRows;

    BriberyEnemy[] enemies;
    List<int> lastRandomValue; 
    bool allFull = false;

    private void Awake()
    {
        enemies = GetComponentsInChildren<BriberyEnemy>();
        List<int> randomIndices = new List<int>(new int[enemies.Length]);
        lastRandomValue = new List<int>(new int[enemies.Length]);

        int random = 0;
        bool first = true;

        for(int i = 0; i < enemies.Length; i++)
        {
            //Set all lastRandomValues to -1 so it doesn't always exclude position 0 from start of money spawn
            lastRandomValue[i] = -1; 

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

        //set start delay, movementspeed, and sendBackDistance randomly here
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].StartDelay = startDelays[randomIndices[i]];
            enemies[i].SetMoveValues(movementSpeeds[randomIndices[i]], sendBackDistances[randomIndices[i]]); 
        }
    }

    private void Start()
    {
        StartMove();
        ActivatedRows = new bool[rows.Length];
        DisabledRows = new bool[rows.Length];
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        base.TrackTime();

        if(currentTime >= moneySpawnDelay)
        {
            currentTime = 0;
            SpawnMoney();
        }
    }

    private void SpawnMoney()
    {
        for(int i = 0; i < ActivatedRows.Length; i++)
        {
            allFull = true;
            if (DisabledRows[i] == true)
            {
                continue;
            }
            else if (ActivatedRows[i] == false)
            {
                allFull = false;
                break;
            }
        }

        if(!allFull)
        {
            for(int i = 0; i < ActivatedRows.Length; i++)
            {
                if(ActivatedRows[i] == false && DisabledRows[i] == false)
                {
                    int random2 = UnityEngine.Random.Range(0, 4); //four per row

                    if (random2 != lastRandomValue[i])
                    {
                        rows[i].GetComponentsInChildren<BriberyCollectable>()[random2].Activate();
                        lastRandomValue[i] = random2;
                    }
                }
            }

        }
    }

    public void IncreaseSpawnTimer()
    {
        moneySpawnDelay += delayIncrement;
    }

    public override void EndMove()
    {
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(bribery);

        if (FindObjectOfType<SixFaceMana>().FaceType == SixFaceMana.FaceTypes.Hype)
            AugmentScore++;
        else if (FindObjectOfType<SixFaceMana>().FaceType == SixFaceMana.FaceTypes.Sad)
            AugmentScore = 0;

        base.EndMove();
        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
