using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDeathZone : MonoBehaviour
{
    PawnStar minigame;

    private void Start()
    {
        minigame = FindObjectOfType<PawnStar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }
}
