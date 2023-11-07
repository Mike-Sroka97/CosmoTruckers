using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeanceVeggie : MonoBehaviour
{
    [SerializeField] int score;

    [SerializeField] float rotateSpeed;
    [SerializeField] float waitTime;
    [SerializeField] float initialUpForce;
    [SerializeField] float xVelocity;
    [SerializeField] float gravity;

    VeggieVengeance minigame;
    AeglarINA aeglar;
    Rigidbody2D myBody;
    DeathParticleSpawner particleSpawner; 
    float currentTime = 0;
    bool trackTime = true;

    private void Start()
    {
        if(xVelocity > 0)
        {
            rotateSpeed = -rotateSpeed;
        }

        myBody = GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<VeggieVengeance>();
        aeglar = FindObjectOfType<AeglarINA>();
        particleSpawner = GetComponent<DeathParticleSpawner>(); 
    }

    private void Update()
    {
        Spin();
        TrackTime();
    }

    private void Spin()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= waitTime)
        {
            trackTime = false;
            myBody.gravityScale = gravity;
            myBody.velocity = new Vector2(xVelocity, 0);
            myBody.AddForce(new Vector2(0, initialUpForce), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<VeggieVengeanceProjectile>())
        {
            minigame.Score += score;
            Debug.Log(minigame.Score);
            if(score < 0)
            {
                aeglar.TakeDamage(); 
            }
            particleSpawner.SpawnDeathParticle(transform); 
            Destroy(gameObject);
        }
    }
}
