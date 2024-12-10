using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongevityOfDog : CombatMove
{
    [SerializeField] GameObject[] touchableShapes;
    [SerializeField] GameObject[] shapesToMake;
    public Color wrongShapeColor; 
    public Color correctShapeColor;
    public Material offMaterial; 

    LoDShapeGenerator shapeGenerator;
    int currentLayout = -1;

    private void Start()
    {
        shapeGenerator = GetComponentInChildren<LoDShapeGenerator>();
        ResetShapes();
    }

    public void ResetShapes()
    {
        currentLayout = UnityEngine.Random.Range(0, touchableShapes.Length);
        shapeGenerator.GenerateLayout(touchableShapes[currentLayout], shapesToMake[currentLayout]);
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        //Calculate Score
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        //Calculate Augment Stacks
        int augmentStacks = Score * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;
        if (CombatManager.Instance.GetCharactersSelected[0].GetComponent<LongDogCharacter>())
            augmentStacks *= 2;

        //Apply augment
        CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(DebuffToAdd, augmentStacks);
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} giving a teammate {Score * augmentStacksPerScore + baseAugmentStacks} stacks of {DebuffToAdd.AugmentName}.";
}
