using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBCircleEnemy : MonoBehaviour
{
    [SerializeField] float verticalSpeed;
    [SerializeField] float horizontalSpeed; 
    [SerializeField] float zigZagOffset;

    float switchMinimum = 0.1f; 
    float rightOffset, leftOffset;
    float startXPosition; 
    float angle;
    bool goingRight = true; 
    Rigidbody2D myBody;


    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        startXPosition = gameObject.transform.position.x;
        rightOffset = startXPosition + zigZagOffset; 
        leftOffset = startXPosition - zigZagOffset;
    }

    private void Update()
    {
        ZigZagMotion(); 
    }

    private void ZigZagMotion()
    {
        float currentXPosition = gameObject.transform.position.x;

        if (goingRight)
        {
            if ((rightOffset - currentXPosition) > switchMinimum)
            {
                myBody.velocity = new Vector2(horizontalSpeed, verticalSpeed);
            }
            else
            {
                goingRight = false; 
            }
        }
        else
        {
            if ((leftOffset - currentXPosition) < switchMinimum)
            {
                myBody.velocity = new Vector2(-horizontalSpeed, verticalSpeed);
            }
            else
            {
                goingRight = true;
            }
        }
    }
}
