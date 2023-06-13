using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastDeath : MonoBehaviour
{
    RageBlast minigame;

    private void Start()
    {
        minigame = FindObjectOfType<RageBlast>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.PlayerDead = true;
        }
    }
}
