using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPostule : MonoBehaviour
{
    [SerializeField] float upForce;

    SuperSlam superSlam;
    SSGozorMovement gozor;
    bool collided = false;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    Rigidbody2D playerBody;

    private void Start()
    {
        superSlam = FindObjectOfType<SuperSlam>();
        gozor = transform.parent.parent.GetComponent<SSGozorMovement>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        playerBody = FindObjectOfType<SafeTINA>().GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && !collided)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                float dotProduct = Vector3.Dot(contact.normal, Vector3.down);

                if (dotProduct > 0.5f) 
                {
                    collided = true;
                    superSlam.Score++;

                    playerBody.GetComponent<SafeTINA>().enabled = true;
                    playerBody.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);

                    gozor.StartMinigame();
                    gozor.mySprites.Remove(myRenderer);
                    gozor.collidersToDisable.Remove(myCollider);
                    StartCoroutine(gozor.FlashMe());
                    myRenderer.enabled = false;
                    Destroy(myCollider);
                    break;
                }
            }
        }
    }
}
