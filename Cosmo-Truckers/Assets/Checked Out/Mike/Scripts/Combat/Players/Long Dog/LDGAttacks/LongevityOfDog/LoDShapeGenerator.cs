using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoDShapeGenerator : MonoBehaviour
{
    [SerializeField] Transform[] shapeSpawns;
    [HideInInspector] public int CorrectShapes = 0;

    int[] shapeIndices = new int[4];
    int currentRandom = -1;

    public void GenerateLayout(GameObject shapes)
    {
        CorrectShapes = 0;
        Instantiate(shapes, transform);
        Transform[] currentShapes = shapes.GetComponentsInChildren<Transform>();
        RandomIndices();
        for(int i = 0; i < shapeSpawns.Length; i++)
        {
            currentShapes[i + 1].transform.localPosition = shapeSpawns[shapeIndices[i] - 1].localPosition;
        }
        Array.Clear(shapeIndices, 0, shapeIndices.Length);
    }

    private void RandomIndices()
    {
        for(int i = 0; i < shapeIndices.Length; i++)
        {
            if(currentRandom == -1)
            {
                currentRandom = UnityEngine.Random.Range(1, shapeIndices.Length + 1);
                shapeIndices[i] = currentRandom;
            }
            else
            {
                while (shapeIndices.Contains(currentRandom))
                {
                    currentRandom = UnityEngine.Random.Range(1, shapeIndices.Length + 1);
                }
                shapeIndices[i] = currentRandom;
            }
        }
    }
}
