using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVDMovementRotationBased : MonoBehaviour
{
    [SerializeField] float distance;
    Collider2D myCollider;
    const int layermask = 1 << 9; //ground
    bool bouncing = false;

    bool canBounceUp = true;
    bool canBounceDown = true;
    bool canBounceRight = true;
    bool canBounceLeft = true;

    private void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Movement();
    }

    private bool GroundCheck(Vector2 direction, bool horizontal)
    {
        if (horizontal)
        {
            return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.x + distance, layermask);
        }
        return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.y + distance, layermask);
    }

    protected void Movement()
    {
        if (bouncing)
            return;

        if (GroundCheck(Vector2.up, false) && canBounceUp)
        {
            SetBounceBools(1);

            if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 90)
            {
                transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
        if (GroundCheck(Vector2.left, false) && canBounceLeft)
        {
            SetBounceBools(2);

            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 180)
            {
                transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
        if (GroundCheck(Vector2.right, true) && canBounceRight)
        {
            SetBounceBools(4);

            if (transform.eulerAngles.z > 270 && transform.eulerAngles.z < 360)
            {
                transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
        if (GroundCheck(Vector2.down, true) && canBounceDown)
        {
            SetBounceBools(3);

            if (transform.eulerAngles.z > 180 && transform.eulerAngles.z < 270)
            {
                transform.Rotate(new Vector3(0, 0, -90));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }

    private void SetBounceBools(int bounceToDisable)
    {
        //1 up; 2 left; 3 down; 4 right;
        if(bounceToDisable == 1)
        {
            canBounceUp = false;
            canBounceDown = true;
            canBounceRight = true;
            canBounceLeft = true;
        }
        else if(bounceToDisable == 2)
        {
            canBounceUp = true;
            canBounceDown = true;
            canBounceRight = true;
            canBounceLeft = false;
        }
        else if(bounceToDisable == 3)
        {
            canBounceUp = true;
            canBounceDown = false;
            canBounceRight = true;
            canBounceLeft = true;
        }
        else
        {
            canBounceUp = true;
            canBounceDown = true;
            canBounceRight = false;
            canBounceLeft = true;
        }
    }
}
