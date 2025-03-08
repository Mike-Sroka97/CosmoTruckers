using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBWaveEnemy : MonoBehaviour
{
    [SerializeField] float xVelocity;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] GameObject deathParticle;

    float particleVerticalCheck = 0.1f; 
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
        if (collision.CompareTag("PlayerAttack"))
        {
            minigame.Score++;
            minigame.CheckSuccess(); 

            GameObject particle = Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
            particle.transform.parent = minigame.transform;

            Transform parent = collision.transform.parent;

            float verticalOffset = transform.position.y - parent.position.y; 

            //rotate vertically instead of horizontally
            if (verticalOffset >= particleVerticalCheck)
            {
                //270f puts it at an upwards angle
                particle.transform.eulerAngles = new Vector3(270f, particle.transform.eulerAngles.y, particle.transform.eulerAngles.z);
            }
            else
            {
                //180f puts it to the left
                if (parent.position.x > transform.position.x)
                {
                    particle.transform.eulerAngles = new Vector3(180f, particle.transform.eulerAngles.y, particle.transform.eulerAngles.z);
                }
            }

            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
    }
}
