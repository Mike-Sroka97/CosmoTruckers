using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPostule : MonoBehaviour
{
    [SerializeField] float upForce;
    [SerializeField] Sprite[] buttonSprites;
    [SerializeField] Material offMaterial; 

    SuperSlam superSlam;
    SSGozorMovement gozor;
    bool collided = false;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    Rigidbody2D playerBody;
    int currentSpriteNumber = 0;
 

    private void Start()
    {
        superSlam = FindObjectOfType<SuperSlam>();
        gozor = transform.parent.parent.GetComponent<SSGozorMovement>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!playerBody)
            playerBody = FindObjectOfType<SafeTINA>().GetComponent<Rigidbody2D>();

        if (collision.transform.tag == "Player" && !collided)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                float dotProduct = Vector3.Dot(contact.normal, Vector3.down);

                if (dotProduct > 0.5f)
                {
                    collided = true;
                    playerBody.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);
                    DamageGonzor();
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerAttack" && !collided)
        {
            collided = true;
            DamageGonzor();
        }
    }

    private void DamageGonzor()
    {
        superSlam.Score++;
        currentSpriteNumber++;

        //Play success sound
        myRenderer.sprite = buttonSprites[currentSpriteNumber];

        if (currentSpriteNumber < (buttonSprites.Length - 1))
        {
            if (superSlam.Score == 1)
            {
                StartCoroutine(gozor.FlashMe(true));
            }
            else
            {
                StartCoroutine(gozor.FlashMe(false));
            }

            collided = false; 
        }
        else
        {
            myRenderer.material = offMaterial; 
            gozor.EarlyEndMinigame(offMaterial);
            superSlam.CheckSuccess();
        }
    }
}
