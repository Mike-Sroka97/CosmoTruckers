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

    Transform fistTransform; 

    bool isGrabbing;
    float timer;


    void Start()
    {
        handCollider.enabled = true;
        fistCollider.enabled = false;
        fistTransform = fistSpriteRenderer.gameObject.transform; 
    }

    void Update()
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
            //Change the sprites
            fistSpriteRenderer.sprite = FistSprites[1];
            fistTransform.localPosition = fistPositions[1];
            //Change the active collider
            handCollider.enabled = false;
            fistCollider.enabled = true;

        }

        if (!grabState)
        {
            //Change the sprites
            fistSpriteRenderer.sprite = FistSprites[0];
            fistTransform.localPosition = fistPositions[0];
            //Change the active collider
            handCollider.enabled = true;
            fistCollider.enabled = false;
            // Disable this collider. Enable it when stretching
            GetComponent<Collider2D>().enabled = false; 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerCollision = collision.GetComponentInParent<Player>();

        if (playerCollision && collision.tag == "Player")
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
