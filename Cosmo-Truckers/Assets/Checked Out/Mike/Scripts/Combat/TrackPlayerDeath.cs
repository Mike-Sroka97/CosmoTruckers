using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerDeath : MonoBehaviour
{
    [SerializeField] bool trackDeath = true;
    [SerializeField] int scoreIncrease = 0;
    [SerializeField] int augmentScoreIncrease = 0;

    [HideInInspector] public bool TrackingDamage = true;

    CombatMove minigame;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    protected void Death()
    {
        if (trackDeath && minigame)
        {
            minigame.PlayerDead = true;
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
                minigame.Score += scoreIncrease;
                minigame.AugmentScore += augmentScoreIncrease;
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
                minigame.Score += scoreIncrease;
                minigame.AugmentScore += augmentScoreIncrease;
                Death();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!TrackingDamage)
            return;

        //Because Players have rigidbodies, it always treats the collision GO as the parent, not the GO with the collider on it 
        if (collision.transform.tag == "Player" && collision.transform.GetComponentInChildren<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                minigame.Score += scoreIncrease;
                minigame.AugmentScore += augmentScoreIncrease;
                Death();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.transform.tag == "Player" && collision.transform.GetComponentInChildren<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            if (!player.iFrames)
            {
                player.TakeDamage();
                minigame.Score += scoreIncrease;
                minigame.AugmentScore += augmentScoreIncrease;
                Death();
            }
        }
    }
}
