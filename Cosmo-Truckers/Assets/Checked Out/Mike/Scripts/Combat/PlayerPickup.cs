using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] int score = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;
    [SerializeField] bool givesAugmentScore = false;
    [SerializeField] bool givesScore = true;
    [HideInInspector] public bool multiplayer;

    [Header("PS on Collect (can be empty)")]
    [SerializeField] ParticleSystem collectParticle; 

    CombatMove minigame;
    bool movingUp = true;
    float startingY;

    private void Start()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
                Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.gameObject.transform);
            }

            Destroy(gameObject);
        }
    }

    public void SetScoringTypes(bool score, bool augScore)
    {
        givesScore = score;
        givesAugmentScore = augScore;
    }

    public void SetScore(int newScore) { score = newScore; }
} 
