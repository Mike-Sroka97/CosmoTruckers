using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerDeath : MonoBehaviour
{
    CombatMove minigame;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }
}
