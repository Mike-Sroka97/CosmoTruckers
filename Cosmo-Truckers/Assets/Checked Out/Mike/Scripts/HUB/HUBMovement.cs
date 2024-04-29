using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.Events;
using System.Linq;

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
    bool control = false;
    bool InChangingRoom = false;
    public bool GetChangingStatus { get => InChangingRoom; }

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
            if (player.name == gameObject.name && player.GetComponent<PlayerManager>().isOwned)
            {
                control = true;
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
        if (onCD || !control) return;
        int x = 0; int y = 0;
        bool canMove = false;

        switch (Input.inputString)
        {
            case "w":
                x = 0;
                y = 1;
                if (transform.position.y < yBounds)
                    canMove = true;
                break;
            case "a":
                x = -1;
                y = 0;
                if (transform.position.x > -xBounds)
                    canMove = true;
                break;
            case "s":
                x = 0;
                y = -1;
                if (transform.position.y > -yBounds)
                    canMove = true;
                break;
            case "d":
                x = 1;
                y = 0;
                if (transform.position.x < xBounds)
                    canMove = true;
                break;

            default:
                x = 0;
                y = 0;
                canMove = false;
                break;
        }

        //Checks if move is valid
        if (CheckLocation(movementBlockers, x, y))
            return;

        //Dimension check
        if (CheckLocation(dimensions, x, y))
        {
            ChangingRoom(false);
            CmdDimensionVote(GetDimention(x, y));
            CmdMove(x, y);
            return;
        }

        if(CheckLocation(changingRooms, x, y) && !InChangingRoom)
        {
            ChangingRoom(true);
            CmdMove(x, y);
            return;
        }

        //Move player
        if (canMove)
        {
            ChangingRoom(false);
            CmdMove(x, y);
        }
    }

    bool CheckLocation(GameObject[] location, int x, int y)
    {
        foreach(GameObject obj in location)
        {
            //Ugly but need to check if player has already voted to enable no collision
            HUBMovement temp;
            if (obj.TryGetComponent<HUBMovement>(out temp) && !temp.isActiveAndEnabled)
                continue;

            if (obj.transform.position.y == transform.position.y + y 
                && obj.transform.position.x == transform.position.x + x)
            {
                return true;
            }
        }

        return false;
    }

    string GetDimention(int x, int y)
    {
        foreach (GameObject obj in dimensions)
        {
            if (obj.transform.position.y == transform.position.y + y && obj.transform.position.x == transform.position.x + x)
            {
                return obj.GetComponent<VoteLocation>().GetDimensionName;
            }
        }

        return null;
    }

    void ChangingRoom(bool value)
    {
        if (value == InChangingRoom) return;

        InChangingRoom = value;
        switch(value)
        {
            case true:
                FindObjectOfType<ChangingRoom>().EnterChangingRoom?.Invoke();
                break;
            case false:
                FindObjectOfType<ChangingRoom>().LeaveChangingRoom?.Invoke();
                break;
        }
    }

    [Command]
    private void CmdDimensionVote(string dimention)
    {
        GetComponent<FunnyWackyDimensionSpin>().enabled = true;
        GetComponent<FunnyWackyDimensionSpin>().CmdSetDimention(dimention);
        //Player vote
        RpcDisable();
    }
    [ClientRpc]
    void RpcDisable()
    {
        this.enabled = false;
    }

    [Command]
    void CmdMove(int x, int y)
    {
        onCD = true;
        transform.position = new Vector3(
            transform.position.x + (moveDistance * x),
            transform.position.y + (moveDistance * y),
            transform.position.z);
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
