using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkanatorHurt : MonoBehaviour
{
    [SerializeField] bool projectile = false;
    [SerializeField] bool spinRight;
    [SerializeField] float rotateSpeed;
    [SerializeField] float xVelocity;
    [SerializeField] float minYVelocity;

    Porkanator minigame;
    Rigidbody2D myBody;
    Collider2D myCollider;

    static int layermask = 11; //player no interaction

    private void Start()
    {
        minigame = FindObjectOfType<Porkanator>();

        if (projectile)
        {
            myBody = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();

            MoveSet();
        }
    }

    private void Update()
    {
        if(projectile)
        {
            RotateMe();
            YCheck();
            MoveSet();
        }
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
        if(collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
        if(collision.transform.name == "Saw Pit")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            myCollider.enabled = false;
            myBody.bodyType = RigidbodyType2D.Static;
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }

    public void SetSpinRight(bool newSpin) { spinRight = newSpin; }
}
