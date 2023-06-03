using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    bool playerAttached = false;
    Rigidbody2D minigame;
    Rigidbody2D playerBody;

    private void Start()
    {
        playerBody = FindObjectOfType<SixfaceINA>().GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<Rigidbody2D>();
    }

    private void Update()
    {
        if(playerAttached)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x + minigame.velocity.x, playerBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerAttached = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerAttached = false;
        }
    }
}
