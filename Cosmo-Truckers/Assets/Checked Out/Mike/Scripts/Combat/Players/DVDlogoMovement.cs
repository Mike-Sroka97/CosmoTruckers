using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVDlogoMovement : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float moveSpeed;

    Rigidbody2D myBody;
    Collider2D myCollider;
    const int layermask = 1 << 9; //ground

    private void Start()
    {
        Initialize();
        RandomStartVelocity();
    }

    protected void Initialize()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Movement();
    }

    protected virtual bool GroundCheck(Vector2 direction, bool horizontal)
    {
        if (horizontal)
        {
            return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.x + distance, layermask);
        }
        return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.y + distance, layermask);
    }

    protected void Movement()
    {
        if (GroundCheck(Vector2.up, false))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeed);
        }
        if (GroundCheck(Vector2.down, false))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, moveSpeed);
        }
        if (GroundCheck(Vector2.right, true))
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        if (GroundCheck(Vector2.left, true))
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
    }

    protected void RandomStartVelocity()
    {
        int random = Random.Range(0, 4);

        switch(random)
        {
            case 0:
                myBody.velocity = new Vector2(moveSpeed, -moveSpeed);
                break;
            case 1:
                myBody.velocity = new Vector2(-moveSpeed, moveSpeed);
                break;
            case 2:
                myBody.velocity = new Vector2(-moveSpeed, moveSpeed);
                break;
            case 3:
                myBody.velocity = new Vector2(moveSpeed, moveSpeed);
                break;
            default:
                break;
        }
    }
}
