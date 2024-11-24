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
    GameObject currentShapeToMake;

    public void GenerateLayout(GameObject shapes, GameObject shapeToMake)
    {
        CorrectShapes = 0;
        Instantiate(shapes, transform);
        if(currentShapeToMake != null)
        {
            Destroy(currentShapeToMake);
        }
        currentShapeToMake = Instantiate(shapeToMake, transform);
        int rotation = UnityEngine.Random.Range(1, 5);
        currentShapeToMake.transform.eulerAngles = new Vector3(0, 0, rotation * 90); // Rotate shape to make randomly by increments of 90 degrees

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
