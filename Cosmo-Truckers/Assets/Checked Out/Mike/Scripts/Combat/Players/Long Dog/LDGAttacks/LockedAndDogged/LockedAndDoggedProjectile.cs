using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedAndDoggedProjectile : MonoBehaviour
{
    [SerializeField] float newGravityScale = 3f;
    [SerializeField] int pointValue;
    [SerializeField] float fallSpeed;
    [SerializeField] int scoreValue;

    Rigidbody2D myBody;
    CircleCollider2D myCollider;
    bool naturallyFalling = false;
    LockedAndDogged minigame;

    private void Start()
    {
        minigame = FindObjectOfType<LockedAndDogged>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if(!naturallyFalling)
        {
            transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("LDGNoInteraction") || collision.transform.CompareTag("Ground"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), myCollider);
        }
        else
        {
            myBody.gravityScale = newGravityScale;
        }

        if(collision.transform.CompareTag("Player"))
        {
            naturallyFalling = true;
            myBody.gravityScale = newGravityScale;
        }
        else if(collision.transform.CompareTag("PlayerNonHurtable"))
        {
            minigame.Score += scoreValue;
            Debug.Log("Your score is: " + minigame.Score);
            Destroy(gameObject);
        }
        else if(collision.transform.CompareTag("PlayerHurtable"))
        {
            Destroy(gameObject);
        }
    }
}
