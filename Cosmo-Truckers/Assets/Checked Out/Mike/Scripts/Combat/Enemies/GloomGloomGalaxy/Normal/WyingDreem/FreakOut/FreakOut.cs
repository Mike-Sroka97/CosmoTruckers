using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOut : CombatMove
{
    [SerializeField] float startDelay = 1f;

    FreakOutSpikeSpawner spawner;
    int lastRandom = -1;

    private void Start()
    {
        StartMove();
        spawner = GetComponentInChildren<FreakOutSpikeSpawner>();
        Invoke("SpawnSpikes", startDelay);
    }

    private void Update()
    {
        if(PlayerDead && !MoveEnded)
        {
            EndMove();
        }
    }

    public void SpawnSpikes()
    {
        bool[] spawns = new bool[9];
        int random = UnityEngine.Random.Range(0, spawns.Length - 1);

        while (lastRandom == random)
        {
            random = UnityEngine.Random.Range(0, spawns.Length - 1);
        }
        lastRandom = random;

        for(int i = 0; i < spawns.Length; i++)
        {
            if(i != random)
            {
                spawns[i] = true;
            }
            else
            {
                spawns[i] = false;
            }
        }

        spawner.SpawnSpikes(spawns);
    }

    public override void EndMove()
    {
        MoveEnded = true;
        Debug.Log(Score);
    }
}
