using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMiserySkeleton : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float moveVelocity;
    [SerializeField] bool movingLeft;
    [SerializeField] float deadTime;

    Rigidbody2D myBody;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    SplitMisery minigame;

    bool dead;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<SplitMisery>();

        if(movingLeft)
        {
            myBody.velocity = new Vector2(-moveVelocity, 0);
        }
        else
        {
            myBody.velocity = new Vector2(moveVelocity, 0);
            myRenderer.flipX = true;
        }
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if (dead)
            return;

        if(transform.position.x > maxX)
        {
            myBody.velocity = new Vector2(-moveVelocity, 0);
            myRenderer.flipX = false;
        }
        else if(transform.position.x < minX)
        {
            myBody.velocity = new Vector2(moveVelocity, 0);
            myRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        myBody.velocity = Vector2.zero;
        dead = true;
        if(!minigame.MoveEnded)
            minigame.Score++;
        Debug.Log("I'm Dead " + minigame.Score);
        myCollider.enabled = false;

        yield return new WaitForSeconds(deadTime);

        dead = false;
        if (!minigame.MoveEnded)
            minigame.Score--;
        Debug.Log("I'm Alive " + minigame.Score);
        myCollider.enabled = true;

        if (transform.position.x < maxX)
        {
            myBody.velocity = new Vector2(moveVelocity, 0);
            myRenderer.flipX = true;
        }
        else if (transform.position.x > minX)
        {
            myBody.velocity = new Vector2(-moveVelocity, 0);
            myRenderer.flipX = false;
        }
    }
}
