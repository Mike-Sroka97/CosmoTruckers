using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] GameObject leader; // in the inspector drag the gameobject the will be following the player to this field
    [SerializeField] int followDistance;

    List<Vector3> storedPositions;
    List<Vector3> storedRotations;


    void Awake()
    {
        storedPositions = new List<Vector3>();
        storedRotations = new List<Vector3>();
    }

    void Update()
    {
        if (storedPositions.Count == 0)
        {
            storedPositions.Add(leader.transform.position); //store the players correct position
            storedRotations.Add(leader.transform.eulerAngles);
            return;
        }
        else if (storedPositions[storedPositions.Count - 1] != leader.transform.position)
        {
            storedPositions.Add(leader.transform.position); //store the position every frame
            storedRotations.Add(leader.transform.eulerAngles);
        }

        if (storedPositions.Count > followDistance)
        {
            transform.position = storedPositions[0]; //move
            transform.eulerAngles = storedRotations[0]; //rotate
            storedPositions.RemoveAt(0); //delete the position that player just move to
            storedRotations.RemoveAt(0);
        }
    }
}
