using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCrustShield : MonoBehaviour
{
    [SerializeField] bool moveRight = true;
    [SerializeField] bool directionRight = true;

    [SerializeField] float oppositeClampUpper;
    [SerializeField] float oppositeClampLower;
    [SerializeField] float oppositeMoveSpeed;
    [SerializeField] bool directionOppositePositive;

    [SerializeField] float lowerClamp;
    [SerializeField] float upperClamp;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        OscillateMe();
        MoveMe();
    }

    private void OscillateMe()
    {
        if(moveRight)
        {
            if(directionOppositePositive)
            {
                transform.position += new Vector3(0, oppositeMoveSpeed * Time.deltaTime, 0);
                if(transform.position.y >= oppositeClampUpper)
                {
                    directionOppositePositive = !directionOppositePositive;
                }
            }
            else
            {
                transform.position -= new Vector3(0, oppositeMoveSpeed * Time.deltaTime, 0);
                if (transform.position.y <= oppositeClampLower)
                {
                    directionOppositePositive = !directionOppositePositive;
                }
            }
        }
        else
        {
            if (directionOppositePositive)
            {
                transform.position += new Vector3(oppositeMoveSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x >= oppositeClampUpper)
                {
                    directionOppositePositive = !directionOppositePositive;
                }
            }
            else
            {
                transform.position -= new Vector3(oppositeMoveSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= oppositeClampLower)
                {
                    directionOppositePositive = !directionOppositePositive;
                }
            }
        }
    }

    private void MoveMe()
    {
        if (moveRight)
        {
            if(directionRight)
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                if(transform.position.x >= upperClamp)
                {
                    directionRight = !directionRight;
                }
            }
            else
            {
                transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= lowerClamp)
                {
                    directionRight = !directionRight;
                }
            }
        }
        else
        {
            if(directionRight)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                if (transform.position.y >= upperClamp)
                {
                    directionRight = !directionRight;
                }
            }
            else
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                if (transform.position.y <= lowerClamp)
                {
                    directionRight = !directionRight;
                }
            }
    }
    }
}
