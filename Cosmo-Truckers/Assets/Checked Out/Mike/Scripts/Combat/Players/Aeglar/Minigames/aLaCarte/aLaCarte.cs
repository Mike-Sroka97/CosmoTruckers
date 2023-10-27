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

    public override List<Character> NoTargetTargeting()
    {
        Targeting tempTargeting = FindObjectOfType<Targeting>();

        int numberOfManaSourcesToSpawn = 3; //three max summons

        for(int i = 8; i <= 11; i++) //checks all enemy summon spots
        {
            if (numberOfManaSourcesToSpawn <= 0)
                break;

            if (EnemyManager.Instance.EnemyCombatSpots[i] == null)
            {
                tempTargeting.TargetEmptySlot(false, i);
                numberOfManaSourcesToSpawn--;
            }
        }

        List<Character> characters = new List<Character>();
        characters.Add(FindObjectOfType<AeglarPlayer>());
        return characters;
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
}
