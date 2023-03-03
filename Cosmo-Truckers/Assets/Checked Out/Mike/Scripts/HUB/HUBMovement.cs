using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.Events;

public class HUBMovement : NetworkBehaviour
{
    [SerializeField] float playerMovementCD = .25f;
    [SerializeField] int xBounds = 4;
    [SerializeField] int yBounds = 3;

    GameObject[] movementBlockers;
    GameObject[] dimensions;
    GameObject[] changingRooms;

    const int moveDistance = 1;
    [SerializeField] bool onCD = false;
    bool firstRotate = true;
    bool rotateLeft = true;
    float currentTime = 0;
    bool controll = false;

    private void Start()
    {
        movementBlockers = GameObject.FindGameObjectsWithTag("MovementBlocker");
        dimensions = GameObject.FindGameObjectsWithTag("Dimension");
        changingRooms = GameObject.FindGameObjectsWithTag("Changing Room");
    }

    public override void OnStartClient()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name == gameObject.name && player.GetComponent<PlayerManager>().hasAuthority)
            {
                controll = true;
                player.GetComponent<PlayerManager>().CmdGivePlayerItem(netIdentity);
            }
        }
    }

    private void Update()
    {
        Move();
        TrackTime();
    }

    private void Move()
    {
        if (onCD || !controll) return;

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
                    CmdMoveLeft();
                    CmdDimensionVote();
                    return;
                }
            }
            //Move left
            if (transform.position.x > -xBounds)
            {
                CmdMoveLeft();
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
                    CmdMoveRight();
                    CmdDimensionVote();
                    return;
                }
            }
            //Moves right
            if (transform.position.x < xBounds)
            {
                CmdMoveRight();
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
                    CmdMoveUp();
                    CmdDimensionVote();
                    return;
                }
            }
            //Moves up
            if (transform.position.y < yBounds)
            {
                CmdMoveUp();
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
                    CmdMoveUp();
                    CmdDimensionVote();
                    return;
                }
            }
            //Moves down
            if (transform.position.y > -yBounds)
            {
                CmdMoveDown();
            }
        }
    }
    [Command]
    private void CmdDimensionVote()
    {
        GetComponent<FunnyWackyDimensionSpin>().enabled = true;
        //Player vote
        this.enabled = false;
    }

    [Command]
    void CmdMoveDown()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance, transform.position.z);
        CmdRotate();
    }

    [Command]
    void CmdMoveUp()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
        CmdRotate();
    }

    [Command]
    void CmdMoveRight()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
        CmdRotate();
    }

    [Command]
    void CmdMoveLeft()
    {
        onCD = true;
        transform.position = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);
        CmdRotate();
    }

    [Command(requiresAuthority = false)]
    void CmdRotate()
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
