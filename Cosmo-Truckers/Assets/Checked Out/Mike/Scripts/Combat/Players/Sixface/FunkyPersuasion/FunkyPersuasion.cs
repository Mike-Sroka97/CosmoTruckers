using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class FunkyPersuasion : CombatMove
{
    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform midSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject midArrow;
    [SerializeField] GameObject rightArrow;
    [SerializeField] float minSpawnTime;
    [SerializeField] float maxSpawnTime;
    [SerializeField] int arrowsToSpawn = 15;

    int lastArrow = -1; 
    float spawnTime;
    float currentSpawnTime;
    int currentNumberOfArrowsSpawned;

    private void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    protected override void Update()
    {
        TrackSpawner();
        base.Update();
    }

    private void TrackSpawner()
    {
        if (!trackTime)
            return;

        currentSpawnTime += Time.deltaTime;

        if(currentSpawnTime >= spawnTime && currentNumberOfArrowsSpawned < arrowsToSpawn)
        {
            currentSpawnTime = 0;
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            SpawnArrow();
        }
    }

    private void SpawnArrow()
    {
        int row = Random.Range(0, 3);

        // Spawning multiple up arrows in a row is problematic
        while (row == lastArrow && lastArrow == 1)
            row = Random.Range(0, 3);

        lastArrow = row;

        switch (row)
        {
            case 0:
                Instantiate(leftArrow, leftSpawn);
                break;
            case 1:
                Instantiate(midArrow, midSpawn);
                break;
            case 2:
                Instantiate(rightArrow, rightSpawn);
                break;
            default:
                break;
        }

        currentNumberOfArrowsSpawned++;
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        Character target = CombatManager.Instance.CharactersSelected[0];
        int subductionToAdd = 2; //base augment to add. Reduce if they are already subdued

        foreach(AugmentStackSO augment in target.GetAUGS)
        {
            if(augment.AugmentName == DebuffToAdd.AugmentName)
            {
                subductionToAdd--;
                break;
            }
        }

        target.AddAugmentStack(DebuffToAdd, subductionToAdd);

        FindObjectOfType<SixFaceMana>().UpdateFace();
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage. The target also received {DebuffToAdd.AugmentName}.";
}
