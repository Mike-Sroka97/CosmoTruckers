using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathHandSuspension : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fistSpriteRenderer; 
    [SerializeField] private Sprite[] FistSprites;
    [SerializeField] private Collider2D handCollider, fistCollider; 
    [SerializeField] private Vector3[] fistPositions;
    [SerializeField] private float fistCloseTime = 2f;
    
    bool isGrabbing;
    float timer;

    private void Start()
    {
        handCollider.enabled = true;
        fistCollider.enabled = false; 
    }

    private void Update()
    {
        GrabbingPlayerCheck();
    }

    private void GrabbingPlayerCheck()
    {
        if (isGrabbing)
        {
            timer += Time.deltaTime;

            if (timer > fistCloseTime)
            {
                isGrabbing = false;
                SetGrabState(false); 
            }
        }
    }
    
    private void SetGrabState(bool grabState)
    {
        if (grabState)
        {
            isGrabbing = true;
            timer = 0;
            handCollider.enabled = false;
            fistCollider.enabled = true; 
            fistSpriteRenderer.sprite = FistSprites[1];
            fistSpriteRenderer.gameObject.transform.localPosition = fistPositions[1];
        }

        if (!grabState)
        {
            fistSpriteRenderer.sprite = FistSprites[0];
            fistSpriteRenderer.gameObject.transform.localPosition = fistPositions[0];
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision)
        {
            SetGrabState(true); 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision && isGrabbing)
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

        if (playerCollision && !isGrabbing)
        {
            SetGrabState(false); 
        }
    }
}
