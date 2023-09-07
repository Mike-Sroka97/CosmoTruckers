using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathHandSuspension : MonoBehaviour
{
    [SerializeField] private Sprite[] FistSprites; 
    private bool isGrabbing;
    [SerializeField] private float fistCloseTime = 2f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision)
        {
            GetComponent<SpriteRenderer>().sprite = FistSprites[1];
        }
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision)
        {
            GetComponent<SpriteRenderer>().sprite = FistSprites[0];
        }


    }
}
