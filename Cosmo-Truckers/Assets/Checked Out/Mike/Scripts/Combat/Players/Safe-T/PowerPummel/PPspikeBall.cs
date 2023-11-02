using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPspikeBall : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float distance;
    [SerializeField] float moveSpeedIncrease;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float speedIncreaseCoolDown;

    Collider2D myCollider;
    Rigidbody2D myBody;
    int layermask = 1 << 9; //ground
    bool trackTime = false;
    float currentTime = 0;
    PowerPummel minigame;

    private void Start()
    {
        GetStartingVariables();
    }

    private void Update()
    {
        CheckForWalls();
        TrackTime();
    }

    private void GetStartingVariables()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<PowerPummel>();
    }
    private void TrackTime()
    {
        if (!trackTime) { return; }

        currentTime += Time.deltaTime;
        if(currentTime >= speedIncreaseCoolDown)
        {
            currentTime = 0;
            trackTime = false;
        }
    }
    private void CheckForWalls()
    {
        if(GroundCheck(Vector2.down))
        {
            if(moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
                myBody.velocity = new Vector2(myBody.velocity.x, moveSpeed);
            }
            else if(trackTime)
            {
                if (myBody.velocity.x > 0)
                {
                    myBody.velocity = new Vector2(moveSpeed, moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(-moveSpeed, moveSpeed);
                }
            }
            else
            {
                moveSpeed += moveSpeedIncrease;
                if (myBody.velocity.x > 0)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x + moveSpeedIncrease, moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(myBody.velocity.x - moveSpeedIncrease, moveSpeed);
                }
                trackTime = true;
            }
        }
        if (GroundCheck(Vector2.up))
        {
            if (moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
                myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeed);
            }
            else if(trackTime)
            {
                if (myBody.velocity.x > 0)
                {
                    myBody.velocity = new Vector2(moveSpeed, -moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(-moveSpeed, -moveSpeed);
                }
            }
            else
            {
                moveSpeed += moveSpeedIncrease;
                if (myBody.velocity.x > 0)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x + moveSpeedIncrease, -moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(myBody.velocity.x - moveSpeedIncrease, -moveSpeed);
                }
                trackTime = true;
            }
        }
        if (GroundCheck(Vector2.right))
        {
            if (moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
                myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
            }
            else if(trackTime)
            {
                if (myBody.velocity.y > 0)
                {
                    myBody.velocity = new Vector2(-moveSpeed, moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(-moveSpeed, -moveSpeed);
                }
            }
            else
            {
                moveSpeed += moveSpeedIncrease;
                if (myBody.velocity.y > 0)
                {
                    myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y + moveSpeedIncrease);
                }
                else
                {
                    myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y - moveSpeedIncrease);
                }
                trackTime = true;
            }
        }
        if (GroundCheck(Vector2.left))
        {
            if (moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
                myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            }
            else if(trackTime)
            {
                if (myBody.velocity.y > 0)
                {
                    myBody.velocity = new Vector2(moveSpeed, moveSpeed);
                }
                else
                {
                    myBody.velocity = new Vector2(moveSpeed, -moveSpeed);
                }
            }
            else
            {
                moveSpeed += moveSpeedIncrease;
                if (myBody.velocity.y > 0)
                {
                    myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y + moveSpeedIncrease);
                }
                else
                {
                    myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y - moveSpeedIncrease);
                }
                trackTime = true;
            }
        }
    }

    private bool GroundCheck(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.y + distance, layermask);

        if (hit)
        {
            if (hit.collider.tag.Equals("LDGNoInteraction"))
            {
                return false;
            }

            return true; 
        }
        else
        {
            return false; 
        }
    }

    public void DetermineStartingMovement()
    {
        if (myBody == null) 
        {
            GetStartingVariables();
        }

        int rightRandom = UnityEngine.Random.Range(0, 2); //coin flip bb
        int upRandom = UnityEngine.Random.Range(0, 2); //coin flip bb

        if (rightRandom == 0)
        {
            myBody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myBody.velocity = new Vector2(-moveSpeed, 0);
        }
        if (upRandom == 0)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, moveSpeed);
        }
        else
        {
            myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeed);
        }
    }
}
