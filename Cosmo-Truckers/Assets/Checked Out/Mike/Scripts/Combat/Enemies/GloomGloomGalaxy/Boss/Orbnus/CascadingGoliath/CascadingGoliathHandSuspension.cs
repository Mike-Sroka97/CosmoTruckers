using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathHandSuspension : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fistSpriteRenderer; 
    [SerializeField] private Sprite[] FistSprites;
    [SerializeField] private Vector3[] fistPositions;
    [SerializeField] private float fistCloseTime = 2f;
    private bool isGrabbing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision)
        {
            fistSpriteRenderer.sprite = FistSprites[1];
            fistSpriteRenderer.gameObject.transform.localPosition = fistPositions[1]; 
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
            fistSpriteRenderer.sprite = FistSprites[0];
            fistSpriteRenderer.gameObject.transform.localPosition = fistPositions[0];
        }


    }
}
