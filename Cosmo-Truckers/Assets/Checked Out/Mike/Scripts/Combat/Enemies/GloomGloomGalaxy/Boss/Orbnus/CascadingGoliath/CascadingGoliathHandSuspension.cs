using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathHandSuspension : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision)
        {
            Rigidbody2D[] playerBodies = playerCollision.GetComponentsInChildren<Rigidbody2D>();

            foreach(Rigidbody2D body in playerBodies)
            {
                body.velocity = Vector2.zero;
            }
        }
    }
}
