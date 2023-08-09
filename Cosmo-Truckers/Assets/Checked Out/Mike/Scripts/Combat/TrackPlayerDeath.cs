using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerDeath : MonoBehaviour
{
    [SerializeField] bool trackDeath = true;

    [HideInInspector] public bool TrackingDamage = true;

    CombatMove minigame;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    private void Death()
    {
        if (trackDeath)
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                Death();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                Death();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.transform.tag == "Player" && collision.transform.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                Death();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.transform.tag == "Player" && collision.transform.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponent<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                Death();
            }
        }
    }
}
