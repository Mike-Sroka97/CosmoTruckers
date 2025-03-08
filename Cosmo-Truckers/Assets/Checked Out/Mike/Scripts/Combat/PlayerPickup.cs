using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] protected int score = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;
    [SerializeField] protected bool givesAugmentScore = false;
    [SerializeField] protected bool givesScore = true;
    [HideInInspector] public bool multiplayer;

    [Header("PS on Collect (can be empty)")]
    [SerializeField] protected ParticleSystem collectParticle; 

    protected CombatMove minigame;
    bool movingUp = true;
    float startingY;

    protected virtual void Start()
    {
        startingY = transform.localPosition.y;
        minigame = FindObjectOfType<CombatMove>();
    }

    private void Update()
    {
        Float();
    }

    private void Float()
    {
        if(movingUp)
        {
            transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if(transform.localPosition.y > startingY + moveDistance)
            {
                movingUp = !movingUp;
            }
        }
        else
        {
            transform.localPosition -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (transform.localPosition.y < startingY - moveDistance)
            {
                movingUp = !movingUp;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(multiplayer)
            {
                Player player = collision.transform.GetComponentInChildren<PlayerBody>().Body;
                
                if (givesScore)
                {
                    minigame.PlayerScores[player.MyCharacter] += score;
                }
                if (givesAugmentScore)
                {
                    minigame.PlayerAugmentScores[player.MyCharacter] += score;
                }
            }
            else
            {
                if (givesScore)
                {
                    minigame.Score += score;
                    minigame.CheckSuccess();
                }

                if (givesAugmentScore)
                {
                    minigame.AugmentScore += score;
                    minigame.CheckAugmentSuccess();
                }

            }

            if (collectParticle != null)
            {
                Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.transform);
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Destroy the pickup even if it hasn't been touched, and still spawn the destroy particle
    /// Gives move visual indication that it is gone
    /// </summary>
    public void DestroyPickup()
    {
        if (collectParticle != null)
        {
            Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.transform);
        }

        Destroy(gameObject);
    }

    public void SetScoringTypes(bool score, bool augScore)
    {
        givesScore = score;
        givesAugmentScore = augScore;
    }

    public void SetScore(int newScore) { score = newScore; }
} 
