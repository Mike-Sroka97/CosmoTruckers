using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBMovement : MonoBehaviour
{
    [SerializeField] float playerMovementCD = .25f;
    [SerializeField] int xBounds = 4;
    [SerializeField] int yBounds = 3;

    const int moveDistance = 1;
    bool onCD = false;
    bool firstRotate = true;
    bool rotateLeft = true;
    float currentTime = 0;

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
                if(transform.position.x > -xBounds)
                {
                    onCD = true;
                    transform.position = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);
                    RotatePlayer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if(transform.position.x < xBounds)
                {
                    onCD = true;
                    transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
                    RotatePlayer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if(transform.position.y < yBounds)
                {
                    onCD = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
                    RotatePlayer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if(transform.position.y > -yBounds)
                {
                    onCD = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y - moveDistance, transform.position.z);
                    RotatePlayer();
                }
            }
        }
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
