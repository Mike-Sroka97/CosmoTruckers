using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBMovement : MonoBehaviour
{
    [SerializeField] float playerMovementCD = .25f;
    [SerializeField] int xBounds = 4;
    [SerializeField] int yBounds = 3;

    GameObject[] movementBlockers;
    GameObject[] dimensions;
    GameObject[] changingRooms;

    const int moveDistance = 1;
    bool onCD = false;
    bool firstRotate = true;
    bool rotateLeft = true;
    float currentTime = 0;

    private void Start()
    {
        movementBlockers = GameObject.FindGameObjectsWithTag("MovementBlocker");
        dimensions = GameObject.FindGameObjectsWithTag("Dimension");
        changingRooms = GameObject.FindGameObjectsWithTag("Changing Room");
    }

    private void Update()
    {
        Move();
        TrackTime();
    }

    private void Move()
    {
        if (!onCD)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Checks if move is valid
                foreach (GameObject movementBlocker in movementBlockers)
                {
                    if(movementBlocker.transform.position.x == transform.position.x - 1 && movementBlocker.transform.position.y == transform.position.y)
                    {
                        return;
                    }
                }
                //Dimension check
                foreach (GameObject dimension in dimensions)
                {
                    if (dimension.transform.position.x == transform.position.x - 1 && dimension.transform.position.y == transform.position.y)
                    {
                        MoveLeft();
                        DimensionVote();
                        return;
                    }
                }
                //Move left
                if (transform.position.x > -xBounds)
                {
                    MoveLeft();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //Checks if move is valid
                foreach (GameObject movementBlocker in movementBlockers)
                {
                    if (movementBlocker.transform.position.x == transform.position.x + 1 && movementBlocker.transform.position.y == transform.position.y)
                    {
                        return;
                    }
                }
                //Dimension check
                foreach (GameObject dimension in dimensions)
                {
                    if (dimension.transform.position.x == transform.position.x + 1 && dimension.transform.position.y == transform.position.y)
                    {
                        MoveRight();
                        DimensionVote();
                        return;
                    }
                }
                //Moves right
                if (transform.position.x < xBounds)
                {
                    MoveRight();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                //Checks if move is valid
                foreach (GameObject movementBlocker in movementBlockers)
                {
                    if (movementBlocker.transform.position.y == transform.position.y + 1 && movementBlocker.transform.position.x == transform.position.x)
                    {
                        return;
                    }
                }
                //Dimension check
                foreach (GameObject dimension in dimensions)
                {
                    if (dimension.transform.position.y == transform.position.y + 1 && dimension.transform.position.x == transform.position.x)
                    {
                        MoveUp();
                        DimensionVote();
                        return;
                    }
                }
                //Moves up
                if (transform.position.y < yBounds)
                {
                    MoveUp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //Checks if move is valid
                foreach (GameObject movementBlocker in movementBlockers)
                {
                    if (movementBlocker.transform.position.y == transform.position.y - 1 && movementBlocker.transform.position.x == transform.position.x)
                    {
                        return;
                    }
                }
                //Dimension check
                foreach (GameObject dimension in dimensions)
                {
                    if (dimension.transform.position.y == transform.position.y - 1 && dimension.transform.position.x == transform.position.x)
                    {
                        MoveUp();
                        DimensionVote();
                        return;
                    }
                }
                //Moves down
                if (transform.position.y > -yBounds)
                {
                    MoveDown();
                }
            }
        }
    }

    private void DimensionVote()
    {
        GetComponent<FunnyWackyDimensionSpin>().enabled = true;
        //Player vote
        this.enabled = false;
    }

    private void MoveDown()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance, transform.position.z);
        RotatePlayer();
    }

    private void MoveUp()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
        RotatePlayer();
    }

    private void MoveRight()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
        RotatePlayer();
    }

    private void MoveLeft()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if (firstRotate)
        {
            transform.Rotate(new Vector3(0, 0, 15));
            firstRotate = false;
        }
        else if (rotateLeft)
        {
            transform.Rotate(new Vector3(0, 0, -30));
            rotateLeft = false;
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 30));
            rotateLeft = true;
        }
    }

    private void TrackTime()
    {
        if(onCD)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= playerMovementCD)
            {
                currentTime = 0;
                onCD = false;
            }
        }
    }
}
