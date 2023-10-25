using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSproutMovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float yDistance;
    [SerializeField] bool movingUp;

    float startingY;
    Rigidbody2D myBody;
    bool checkDistance = false;

    public void StartMove()
    {
        startingY = transform.position.y;
        myBody = GetComponent<Rigidbody2D>();

        if (movingUp)
        {
            myBody.velocity = new Vector2(0, moveSpeed);
        }
        else
        {
            myBody.velocity = new Vector2(0, -moveSpeed);
        }

        checkDistance = true;
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (!checkDistance)
            return;

        if(movingUp)
        {
            if(startingY + yDistance < transform.position.y)
            {
                movingUp = !movingUp;
                myBody.velocity = new Vector2(0, -moveSpeed);
            }
        }
        else
        {
            if (startingY - yDistance > transform.position.y)
            {
                movingUp = !movingUp;
                myBody.velocity = new Vector2(0, moveSpeed);
            }
        }
    }
}
