using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSproutMovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float yDistance;
    [SerializeField] bool movingUp;
    [SerializeField] float minStartingY = -5f;
    [SerializeField] float maxStartingY = -2f;

    float startingY;
    Rigidbody2D myBody;
    bool checkDistance = false;

    private void Start()
    {
        startingY = Random.Range(minStartingY, maxStartingY);
        transform.localPosition = new Vector3(transform.localPosition.x, startingY, transform.localPosition.z);
    }

    public void StartMove()
    {
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
            if(startingY + yDistance < transform.localPosition.y)
            {
                movingUp = !movingUp;
                myBody.velocity = new Vector2(0, -moveSpeed);
            }
        }
        else
        {
            if (startingY - yDistance > transform.localPosition.y)
            {
                movingUp = !movingUp;
                myBody.velocity = new Vector2(0, moveSpeed);
            }
        }
    }
}
