using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aLaCarte : CombatMove
{
    [SerializeField] Transform[] collectibleSpawns;
    [SerializeField] Transform[] collectibles;
    [SerializeField] GameObject veggieSummon;
    [SerializeField] GameObject meatSummon;
    [SerializeField] GameObject sweetSummon;

    int noSpawnIndex = 4; //starts as four as this is the spawn for Aeglar
    int currentNumberOfCollectiblesSpawned = 0;
    bool[] spotsTaken;

    const int veggieScore = 3;
    const int meatScore = 6;
    const int sweetScore = 9;

    int numberOfManaSourcesToSpawn;

    private void Start()
    {
        spotsTaken = new bool[collectibleSpawns.Length];
        GenerateCurrentLayout();
    }

    public override List<Character> NoTargetTargeting()
    {
        numberOfManaSourcesToSpawn = 3; //three max summons

        for(int i = 8; i <= 11; i++) //checks all enemy summon spots
        {
            if (numberOfManaSourcesToSpawn <= 0)
                break;

            if (EnemyManager.Instance.EnemyCombatSpots[i] == null)
            {
                CombatManager.Instance.MyTargeting.TargetEmptySlot(false, i);
                numberOfManaSourcesToSpawn--;
            }
        }

        List<Character> characters = new List<Character>();
        characters.Add(FindObjectOfType<AeglarCharacter>());
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
                collectibles[currentNumberOfCollectiblesSpawned].position = collectibleSpawns[random].position;
                currentNumberOfCollectiblesSpawned++;
            }
        }

        currentNumberOfCollectiblesSpawned = 0;
        noSpawnIndex = tempSpawnIndex;
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        AeglarMana aeglarMana = FindObjectOfType<AeglarMana>();

        if(Score >= veggieScore)
            aeglarMana.AdjustMana(1, 0);
        if(Score >= meatScore)
            aeglarMana.AdjustMana(1, 1);
        if(Score >= sweetScore)
            aeglarMana.AdjustMana(1, 2);

        for(int i = 0; i < 3 - numberOfManaSourcesToSpawn; i++)
        {
            switch(i)
            {
                case 0:
                    EnemyManager.Instance.UpdateEnemySummons(veggieSummon);
                    break;
                case 1:
                    EnemyManager.Instance.UpdateEnemySummons(meatSummon);
                    break;
                case 2:
                    EnemyManager.Instance.UpdateEnemySummons(sweetSummon);
                    break;
                default:
                    break;
            }
        }
    }
}
