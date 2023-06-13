using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapMovePlayer : MonoBehaviour
{
    [SerializeField] PaPConveyor myConveyor;

    Transform playerInitialParent;
    SafeTINA player;

    private void Start()
    {
        player = FindObjectOfType<SafeTINA>();
        playerInitialParent = player.transform.parent;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.transform.parent = playerInitialParent;
        }
    }
}
