using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapMovePlayer : MonoBehaviour
{
    [SerializeField] PaPConveyor myConveyor;

    Rigidbody2D playerBody;
    SafeTINA player;

    private void Start()
    {
        player = FindObjectOfType<SafeTINA>();
        playerBody = player.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.PlatformMoveMe = true;
            if (Input.GetKey(KeyCode.D) && !player.GetIsJumping())
            {
                playerBody.velocity = new Vector2(player.GetMoveSpeed() * 1.5f, playerBody.velocity.y);
            }
            else if(Input.GetKey(KeyCode.A) && !player.GetIsJumping())
            {
                playerBody.velocity = new Vector2(-player.GetMoveSpeed() / 2, playerBody.velocity.y);
            }
            else 
            {
                playerBody.velocity = new Vector2(myConveyor.GetMoveSpeed(), playerBody.velocity.y);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.PlatformMoveMe = false;
    }
}
