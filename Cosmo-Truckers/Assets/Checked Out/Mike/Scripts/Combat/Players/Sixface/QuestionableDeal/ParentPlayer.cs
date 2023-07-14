using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    bool playerAttached = false;
    Rigidbody2D minigame;
    Rigidbody2D playerBody;
    int layermask = 1 << 6;
    Collider2D myCollider; 

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        playerBody = FindObjectOfType<SixfaceINA>().GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<Rigidbody2D>();
    }

    private void Update()
    {
        if(Grounded())
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x + minigame.velocity.x, playerBody.velocity.y);
        }
    }

    public bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, myCollider.bounds.extents.y + .05f, layermask);
    }


}
