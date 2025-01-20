using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkanatorHurt : MonoBehaviour
{
    [SerializeField] bool spinRight;
    [SerializeField] float rotateSpeed;
    [SerializeField] float xVelocity;
    [SerializeField] float minYVelocity;

    Porkanator minigame;
    Rigidbody2D myBody;
    ParticleSpawner myParticleSpawner;
    Collider2D myCollider;

    private void Start()
    {
        minigame = FindObjectOfType<Porkanator>();

        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myParticleSpawner = GetComponent<ParticleSpawner>(); 

        MoveSet();
    }

    private void Update()
    {
        RotateMe();
        YCheck();
        MoveSet();
    }

    private void RotateMe()
    {
        if(spinRight)
        {
            transform.Rotate(0, 0, rotateSpeed * -Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }

    }

    private void YCheck()
    {
        if(myBody.velocity.y < minYVelocity)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, minYVelocity);
        }
    }

    private void MoveSet()
    {
        if (spinRight)
        {
            myBody.velocity = new Vector2(xVelocity, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(-xVelocity, myBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Saw Pit")
        {
            myParticleSpawner.SpawnParticle(transform, true); 
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            myParticleSpawner.SpawnParticle(transform, true);
            Destroy(gameObject);
        }
    }

    public void SetSpinRight(bool newSpin) { spinRight = newSpin; }
}
