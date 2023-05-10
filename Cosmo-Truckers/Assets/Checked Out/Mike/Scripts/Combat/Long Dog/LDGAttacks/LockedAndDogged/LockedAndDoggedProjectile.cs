using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedAndDoggedProjectile : MonoBehaviour
{
    [SerializeField] float newGravityScale = 3f;
    [SerializeField] bool goodProjectile;
    [SerializeField] int pointValue;
    [SerializeField] float fallSpeed;

    Rigidbody2D myBody;
    CircleCollider2D myCollider;
    bool naturallyFalling = false;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
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
        if(collision.transform.tag == "LDGNoInteraction")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), myCollider);
        }

        if(collision.transform.tag == "Player")
        {
            naturallyFalling = true;
            myBody.gravityScale = newGravityScale;
        }
        else if(collision.transform.tag == "PlayerNonHurtable")
        {
            //score++ for good
            //scoree-- for bad
            Destroy(gameObject);
        }
        else if(collision.transform.tag == "PlayerHurtable")
        {
            //score-- for good
            //scoree++ for bad
            Destroy(gameObject);
        }
    }
}
