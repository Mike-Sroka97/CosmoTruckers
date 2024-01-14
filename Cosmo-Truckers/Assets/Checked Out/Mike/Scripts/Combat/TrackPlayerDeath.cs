using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerDeath : MonoBehaviour
{
    [SerializeField] bool trackDeath = true;
    [SerializeField] int scoreIncrease = 0;
    [SerializeField] int augmentScoreIncrease = 0;
    [SerializeField] bool multiplayer = false;

    [HideInInspector] public bool TrackingDamage = true;

    CombatMove minigame;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    protected void Death(Player player)
    {
        if (trackDeath && minigame)
        {
            if (multiplayer)
            {
                player.dead = true;

                minigame.PlayersDead[player.MyCharacter] = true;

                bool allDead = true;
                foreach(KeyValuePair<PlayerCharacter, bool> entry in minigame.PlayersDead)
                    if (!entry.Value)
                        allDead = false;

                if(allDead)
                    minigame.PlayerDead = true;
            }

            else
                minigame.PlayerDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            AdjustMinigameScore(player);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.tag == "Player" && collision.GetComponent<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            AdjustMinigameScore(player);
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
            AdjustMinigameScore(player);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!TrackingDamage)
            return;

        if (collision.transform.tag == "Player" && collision.transform.GetComponentInChildren<PlayerBody>())
        {
            Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
            AdjustMinigameScore(player);
        }
    }

    private void AdjustMinigameScore(Player player)
    {
        if (multiplayer)
        {
            if (!player.iFrames)
            {
                Death(player);
                player.TakeDamage();
                minigame.PlayerScores[player.MyCharacter] += scoreIncrease;
                minigame.PlayerAugmentScores[player.MyCharacter] += augmentScoreIncrease;
            }
        }
        else
        {
            if (!player.iFrames)
            {
                Death(player);
                player.TakeDamage();
                minigame.Score += scoreIncrease;
                minigame.AugmentScore += augmentScoreIncrease;
            }
        }
    }
}
