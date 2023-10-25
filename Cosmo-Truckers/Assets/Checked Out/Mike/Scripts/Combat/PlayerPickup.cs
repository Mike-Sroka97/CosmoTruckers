using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] int score = 1;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDistance;
    [SerializeField] bool endsMinigame;

    [Header("PS on Collect (can be empty)")]
    [SerializeField] ParticleSystem collectParticle; 

    CombatMove minigame;
    bool movingUp = true;
    float startingY;

    private void Start()
    {
        startingY = transform.position.y;
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
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if(transform.position.y > startingY + moveDistance)
            {
                movingUp = !movingUp;
            }
        }
        else
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (transform.position.y < startingY - moveDistance)
            {
                movingUp = !movingUp;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minigame.Score += score;
            Debug.Log(minigame.Score);

            if(endsMinigame)
            {
                minigame.EndMove();
            }

            if (collectParticle != null)
            {
                ParticleSystem particle = Instantiate(collectParticle, gameObject.transform);
                particle.gameObject.transform.parent = minigame.gameObject.transform; 
            }

            Destroy(gameObject);
        }
    }
}
