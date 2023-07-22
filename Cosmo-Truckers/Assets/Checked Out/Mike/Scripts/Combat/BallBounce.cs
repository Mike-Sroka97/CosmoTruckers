using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D myBody;

    bool goingRight = true;
    bool goingUp = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            goingRight = false;
        }
        random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            goingUp = false;
        }

        SetVelocity();
    }

    private void SetVelocity()
    {
        if (goingRight && goingUp)
        {
            myBody.velocity = new Vector2(moveSpeed, moveSpeed);
        }
        else if (goingRight && !goingUp)
        {
            myBody.velocity = new Vector2(moveSpeed, -moveSpeed);
        }
        else if (!goingRight && !goingUp)
        {
            myBody.velocity = new Vector2(-moveSpeed, -moveSpeed);
        }
        else
        {
            myBody.velocity = new Vector2(-moveSpeed, moveSpeed);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 center = transform.position;
        Vector3 direction = collisionPoint - center;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        angle += 45;
        if (angle > 360) angle -= 360;

        int quadrant = (int)(angle / 90) + 1;

        switch (quadrant)
        {
            case 1:
                goingRight = false;
                break;
            case 2:
                goingUp = false;
                break;
            case 3:
                goingRight = true;
                break;
            case 4:
                goingUp = true;
                break;
            default:
                break;
        }

        SetVelocity();
    }
}
