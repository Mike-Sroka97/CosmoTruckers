using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COKOhand : MonoBehaviour
{
    [SerializeField] float velocity = 0;
    [SerializeField] float damagingSpeed;
    [SerializeField] float damagedDuration;
    [SerializeField] float resetPosition;
    [SerializeField] bool horizontalMatters;
    [SerializeField] bool right;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Collider2D[] allColliders;
    [SerializeField] Rigidbody2D[] bodiesToMove;

    Rigidbody2D myBody;
    bool playerDamaged = false;

    public void SetVelocity()
    {
        myBody = GetComponent<Rigidbody2D>();
        if (horizontalMatters)
        {
            myBody.velocity = new Vector2(velocity, 0);
        }
        else
        {
            myBody.velocity = new Vector2(0, velocity);
        }
    }

    private void Update()
    {
        TeleportCheck();
    }

    public IEnumerator PlayerHit()
    {
        if (!playerDamaged)
        {
            playerDamaged = true;
            foreach (Collider2D collider in allColliders)
            {
                collider.enabled = false;
            }
            foreach (Rigidbody2D body in bodiesToMove)
            {
                if (horizontalMatters)
                {
                    body.velocity = new Vector2(damagingSpeed, 0);
                }
                else
                {
                    body.velocity = new Vector2(0, damagingSpeed);
                }
            }

            yield return new WaitForSeconds(damagedDuration);

            foreach (Collider2D collider in allColliders)
            {
                collider.enabled = true;
            }
            foreach (Rigidbody2D body in bodiesToMove)
            {
                if (horizontalMatters)
                {
                    body.velocity = new Vector2(velocity, 0);
                }
                else
                {
                    body.velocity = new Vector2(0, damagingSpeed);
                }
            }
            playerDamaged = false;
        }
    }

    private void TeleportCheck()
    {
        if(horizontalMatters && right)
        {
            if (transform.localPosition.x > resetPosition)
            {
                transform.localPosition = startingPosition;
            }
        }
        else if(horizontalMatters && !right)
        {
            if(transform.localPosition.x < resetPosition)
            {
                transform.localPosition = startingPosition;
            }
        }
        else if(!horizontalMatters)
        {
            if(transform.localPosition.y < resetPosition)
            {
                transform.localPosition = startingPosition;
            }
        }
    }
}
