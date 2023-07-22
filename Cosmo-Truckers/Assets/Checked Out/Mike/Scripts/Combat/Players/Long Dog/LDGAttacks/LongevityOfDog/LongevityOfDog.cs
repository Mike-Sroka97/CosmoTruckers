using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongevityOfDog : CombatMove
{
    [SerializeField] GameObject[] touchableShapes;
    [SerializeField] int maxLayouts;
    [SerializeField] GameObject[] shapesToMake;

    List<int> generatedLayouts = new List<int>();
    LoDShapeGenerator shapeGenerator;
    int currentLayout = -1;
    int numberOfLayouts = 0;

    private void Start()
    {
        StartMove();
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
        throw new System.NotImplementedException();
    }
}
