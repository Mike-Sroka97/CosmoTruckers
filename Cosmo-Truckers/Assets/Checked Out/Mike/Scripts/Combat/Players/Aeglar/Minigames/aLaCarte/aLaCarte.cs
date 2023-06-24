using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALaCarte : CombatMove
{
    [SerializeField] Transform[] collectibleSpawns;
    [SerializeField] aLaCarteCollectible[] collectibles;

    int noSpawnIndex = 4; //starts as four as this is the spawn for Aeglar
    int currentNumberOfCollectiblesSpawned = 0;
    bool[] spotsTaken;

    private void Start()
    {
        StartMove();
        spotsTaken = new bool[collectibleSpawns.Length];
        GenerateCurrentLayout();
    }

    public void GenerateCurrentLayout()
    {
        int tempSpawnIndex = 0;

        for(int i = 0; i < spotsTaken.Length; i++)
        {
            spotsTaken[i] = false;
        }

        while (currentNumberOfCollectiblesSpawned < collectibles.Length)
        {
            int random = UnityEngine.Random.Range(0, collectibleSpawns.Length);
            if (spotsTaken[random] == false && random != noSpawnIndex)
            {
                if(currentNumberOfCollectiblesSpawned == 0)
                {
                    tempSpawnIndex = random;
                }

                spotsTaken[random] = true;
                collectibles[currentNumberOfCollectiblesSpawned].transform.position = collectibleSpawns[random].position;
                currentNumberOfCollectiblesSpawned++;
            }
        }

        currentNumberOfCollectiblesSpawned = 0;
        noSpawnIndex = tempSpawnIndex;
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
