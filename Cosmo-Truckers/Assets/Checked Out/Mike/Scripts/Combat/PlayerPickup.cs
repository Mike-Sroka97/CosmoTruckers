using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] int score = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;
    [SerializeField] bool givesAugmentScore = false;

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
            if (!givesAugmentScore)
                minigame.Score += score;
            else
                minigame.AugmentScore += score;

            if (collectParticle != null)
            {
                ParticleSystem particle = Instantiate(collectParticle, transform.position, collectParticle.transform.rotation, minigame.gameObject.transform);
            }

            Destroy(gameObject);
        }
    }
}
