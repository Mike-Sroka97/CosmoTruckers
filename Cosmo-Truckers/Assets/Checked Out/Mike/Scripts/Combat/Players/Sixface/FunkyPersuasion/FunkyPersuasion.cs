using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    List<int> Arrows = new List<int>();

    float spawnTime;
    float currentSpawnTime;
    int currentNumberOfArrowsSpawned = 0;

    private void Start()
    {
        CreateArrowList();
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

    // Actually spawn the arrows
    private void SpawnArrow()
    {
        int row = Arrows[currentNumberOfArrowsSpawned]; 

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

    // Create a list of ints to use for spawning arrow types
    private void CreateArrowList()
    {
        int currentArrow = 0;
        int lastArrow = -1;
        int duplicateRow = 0; 

        for (int i = 0; i < arrowsToSpawn; i++)
        {
            currentArrow = Random.Range(0, 3);

            // Problems occur with vertical arrows spawning more than one in a row
            if (currentArrow == 1 && lastArrow == 1)
                currentArrow = (MathHelpers.RandomBool() == true) ? 2 : 0;

            // Check if arrows in the row are the same as previous and if there have been arrows in the same row more than twice in a row
            while (currentArrow == lastArrow && duplicateRow >= 2)
                currentArrow = Random.Range(0, 3);

            if (currentArrow != lastArrow)
            {
                duplicateRow = 0;
            }
            else
            {
                duplicateRow++; 
            }

            // Set the last arrow to the current arrow
            lastArrow = currentArrow;

            // Add this arrow to the list
            Arrows.Add(currentArrow);
        }
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
