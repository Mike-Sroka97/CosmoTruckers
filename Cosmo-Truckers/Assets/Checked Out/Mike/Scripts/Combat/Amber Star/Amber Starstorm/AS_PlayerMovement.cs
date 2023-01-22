using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform[] playerPositions;
    [SerializeField] float playerMovementCD;

    bool OnCD = false;
    int playerPosition = 2; //randomize???
    float currentTime = 0;

    private void Update()
    {
        Move();
        TrackCD();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.A) && transform.position.x > playerPositions[0].position.x)
        {
            OnCD = true;
            playerPosition--;
            transform.position = playerPositions[playerPosition].position;
        }
        else if (Input.GetKeyDown(KeyCode.D) && transform.position.x < playerPositions[playerPositions.Length - 1].position.x)
        {
            OnCD = true;
            playerPosition++;
            transform.position = playerPositions[playerPosition].position;
        }
    }
    private void TrackCD()
    {
        if (OnCD)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= playerMovementCD)
            {
                currentTime = 0;
                OnCD = false;
            }
        }
    }
}
