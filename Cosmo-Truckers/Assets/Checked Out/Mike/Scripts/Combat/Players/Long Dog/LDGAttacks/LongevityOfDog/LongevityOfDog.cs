using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongevityOfDog : CombatMove
{
    [SerializeField] GameObject[] touchableShapes;
    [SerializeField] int maxLayouts;
    [SerializeField] GameObject[] shapesToMake;
    public Color wrongShapeColor; 
    public Color correctShapeColor;
    public Material offMaterial; 

    List<int> generatedLayouts = new List<int>();
    LoDShapeGenerator shapeGenerator;
    int currentLayout = -1;
    int numberOfLayouts = 0;

    private void Start()
    {
        shapeGenerator = GetComponentInChildren<LoDShapeGenerator>();
        ResetShapes();
    }

    public void ResetShapes()
    {
        if(numberOfLayouts < maxLayouts)
        {
            RandomLayout();
            shapeGenerator.GenerateLayout(touchableShapes[currentLayout], shapesToMake[currentLayout]);
        }
    }

    private void RandomLayout()
    {
        if(currentLayout == -1)
        {
            currentLayout = UnityEngine.Random.Range(0, touchableShapes.Length);
            generatedLayouts.Add(currentLayout);
        }
        else
        {
            while(generatedLayouts.Contains(currentLayout))
            {
                currentLayout = UnityEngine.Random.Range(0, touchableShapes.Length);
            }
            generatedLayouts.Add(currentLayout);
        }
        numberOfLayouts++;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        //Calculate Damage
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
}
