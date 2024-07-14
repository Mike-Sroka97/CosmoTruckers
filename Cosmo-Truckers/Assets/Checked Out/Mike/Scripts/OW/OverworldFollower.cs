using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldFollower : MonoBehaviour
{
    [SerializeField] GameObject leader; // in the inspector drag the gameobject the will be following the player to this field
    [SerializeField] int followDistance;

    SpriteRenderer myRenderer;
    List<Vector3> storedPositions;
    bool overflow;
    Overworld overworld;

    void Awake()
    {
        storedPositions = new List<Vector3>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        overworld = FindObjectOfType<Overworld>();
        transform.position = overworld.Nodes[0].transform.position;
    }

    void Update()
    {
        Flip();
        Move();
    }

    private void Move()
    {
        if (storedPositions.Count == 0)
        {
            storedPositions.Add(leader.transform.position); //store the players correct position
            return;
        }
        else if (storedPositions[storedPositions.Count - 1] != leader.transform.position)
        {
            storedPositions.Add(leader.transform.position); //store the position every frame
        }

        if (storedPositions.Count > followDistance)
        {
            transform.position = storedPositions[0]; //move
            storedPositions.RemoveAt(0); //delete the position that player just move to
            overflow = true;
        }
        else if (overflow && storedPositions.Count >= 0)
        {
            transform.position = storedPositions[0]; //move
            storedPositions.RemoveAt(0); //delete the position that player just move to

            if (storedPositions.Count == 0)
            {
                overflow = false;
                myRenderer.flipX = leader.GetComponentInChildren<SpriteRenderer>().flipX;
            }
        }
    }

    private void Flip()
    {
        if (leader.transform.position.x > transform.position.x)
            myRenderer.flipX = false;
        else
            myRenderer.flipX = true;
    }
}
