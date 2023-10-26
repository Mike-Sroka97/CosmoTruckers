using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBWaveEnemy : MonoBehaviour
{
    [SerializeField] float xVelocity;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] GameObject deathParticle; 

    Rigidbody2D myBody;
    CounterBop minigame;

    float startTime;

    private void Start()
    {
        startTime = Time.time;
        minigame = FindObjectOfType<CounterBop>();
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(xVelocity, myBody.velocity.y);
    }

    private void Update()
    {
        Oscillate();
    }

    private void Oscillate()
    {
        float value = Mathf.Sin((Time.time - startTime) * frequency) * amplitude;
        myBody.velocity = new Vector2(xVelocity, value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            minigame.Score++;

            GameObject particle = Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
            particle.transform.parent = minigame.transform;

            if (collision.transform.position.x > transform.position.x)
            {
                particle.transform.eulerAngles = new Vector3(180f, particle.transform.eulerAngles.y, particle.transform.eulerAngles.z); 
            }

            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
    }
}
