using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerDeath : MonoBehaviour
{
    [SerializeField] bool trackDeath = true;
    [Header("Score/Aug Increase actually decreases (NOT TRUE)")]
    [SerializeField] int scoreIncrease = 0;
    [SerializeField] int augmentScoreIncrease = 0;
    [SerializeField] int overrideDamage = 0;
    public bool Multiplayer = false;
    public bool Boss = false;
    [HideInInspector] public bool TrackingDamage = true;

    CombatMove minigame;
    int damage;

    private void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
        if (overrideDamage != 0)
            damage = overrideDamage;
        else
            damage = minigame.Damage;
    }

    protected void Death(Player player)
    {
        if (trackDeath && minigame)
        {
            if(player.MyCharacter.MyVessel)
                player.MyCharacter.MyVessel.UpdateSprites();

            if (Multiplayer)
            {
                if (Boss && player.MyCharacter.CurrentHealth > 0)
                    return;

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
        if(Boss)
        {
            if (!player.iFrames)
            {
                KeyValuePair<bool, int> damageValues = minigame.CalculateDamage(damage); 

                player.MyCharacter.SwitchCombatOutcomes(EnumManager.CombatOutcome.Damage, damageValues.Value, damageValues.Key);
                player.TakeDamage();
                player.MyCharacter.MyVessel.AdjustCurrentHealthDisplay(player.MyCharacter.CurrentHealth); 
                player.MyCharacter.MyVessel.AdjustShieldDisplay(player.MyCharacter.Shield); 
                Death(player);
            }
        }
        else if (Multiplayer)
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
