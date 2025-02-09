using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encrustable : CombatMove
{
    [SerializeField] Transform[] blockSpawns;
    [SerializeField] GameObject breakableBlock;
    [SerializeField] GameObject breakableBomb;
    [SerializeField] GameObject breakableHeart;
    [SerializeField] float numberOfBasicBlocks = 58; //fuck you
    [SerializeField] float numberOfBombs = 12; //eat shit
    [SerializeField] float numberOfHearts = 3; // <3
    [SerializeField] float numberOfOpenSlots = 10; //fuck you

    int currentBasics = 0;
    int currentBombs = 0;
    int currentHearts = 0;
    int currentOpen = 0;

    private void Start()
    {
        BuildBlocks();
    }

    /// <summary>
    /// uhoh here i go writing algorithms to make the game fun again
    /// </summary>
    private void BuildBlocks()
    {
        //generate blocks
        for (int i = 0; i < blockSpawns.Length; i++)
            GenerateBlock(GetNextBlockType(i), i);
    }

    /// <summary>
    /// sets limits on block types
    /// </summary>
    /// <returns></returns>
    private int GetNextBlockType(int index)
    {
        int random = Random.Range(0, 100);

        random = CorrectForWeights(random, index);

        if (random == 0 && currentBasics >= numberOfBasicBlocks)
            random = GetNextAvailableBlock();
        if (random == 1 && currentBombs >= numberOfBombs)
            random = GetNextAvailableBlock();
        if (random == 2 && currentHearts >= numberOfHearts)
            random = GetNextAvailableBlock();
        if (random == 3 && currentOpen >= numberOfOpenSlots)
            random = GetNextAvailableBlock();

        return random;
    }

    /// <summary>
    /// Uses weight for more balanced random generation
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    private int CorrectForWeights(int weight, int currentIndex)
    {
        float basicWeight = (numberOfBasicBlocks / blockSpawns.Length) * 75;
        float bombWeight = (numberOfBombs / blockSpawns.Length) * 100;
        float heartWeight = (numberOfHearts / blockSpawns.Length) * 200;

        if (weight < basicWeight)
            return 0; //basic
        else if (weight >= basicWeight && weight < basicWeight + bombWeight)
        {
            if (!DetermineHeartRow(currentIndex))
                return 1; //bomb
            else
                return 2; //heart
        }

        else if (weight >= basicWeight + bombWeight && weight < basicWeight + bombWeight + heartWeight)
        {
            if (!DetermineHeartRow(currentIndex))
                return 1; //bomb
            else
                return 2; //heart
        }
        else
            return 3; //nothing -- nothing? yea a pie with nothing on it
    }

    private bool DetermineHeartRow(int index)
    {
        return (index <= 11 || (index > 23 && index <= 36) || (index > 47 && index <= 59) || (index > 71 && index <= 83));
    }

    /// <summary>
    /// Gets the next available block for spawning
    /// </summary>
    /// <returns></returns>
    private int GetNextAvailableBlock()
    {
        if (currentBasics < numberOfBasicBlocks)
            return 0;
        else if (currentOpen < numberOfOpenSlots)
            return 3;
        else if (currentBombs < numberOfBombs)
            return 1;
        else
            return 2;
    }


    /// <summary>
    /// Instantiate block
    /// </summary>
    /// <param name="type"></param>
    private void GenerateBlock(int type, int spawnIndex)
    {
        switch(type)
        {
            case 0:
                currentBasics++;
                Instantiate(breakableBlock, blockSpawns[spawnIndex]);
                break;
            case 1:
                currentBombs++;
                Instantiate(breakableBomb, blockSpawns[spawnIndex]);
                break;
            case 2:
                currentHearts++;
                Instantiate(breakableHeart, blockSpawns[spawnIndex]);
                break;
            case 3:
                currentOpen++;
                break;
            default:
                break;
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        int stacks = CalculateAugmentScore();

        ApplyAugment(CombatManager.Instance.CharactersSelected[0], stacks);
    }
}
