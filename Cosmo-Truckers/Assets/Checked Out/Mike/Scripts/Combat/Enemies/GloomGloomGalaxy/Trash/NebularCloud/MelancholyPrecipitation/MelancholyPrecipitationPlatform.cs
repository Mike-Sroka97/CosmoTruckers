using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitationPlatform : MonoBehaviour
{
    [SerializeField] float shakeXBounds;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeSpeed;

    [SerializeField] float fallSpeed;
    [SerializeField] float yLimit;

    bool movingLeft = true;
    float currentTime = 0;
    bool trackTime = false;
    bool falling;
    Collider2D myCollider;
    Vector3 startingPosition;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        startingPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            trackTime = true;
        }
    }

    private void Update()
    {
        Fall();
        TrackTime();
    }

    private void Fall()
    {
        if (!falling)
            return;

        myCollider.enabled = false;
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);

        if(transform.position.y < yLimit)
        {
            Destroy(gameObject);
        }
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;
        Shake();

        if(currentTime >= shakeDuration)
        {
            falling = true;
            trackTime = false;
        }
    }

    private void Shake()
    {
        if(movingLeft)
        {
            transform.position -= new Vector3(shakeSpeed * Time.deltaTime, 0, 0);
            if(transform.position.x < startingPosition.x - shakeXBounds)
            {
                movingLeft = !movingLeft;
            }
        }
        else
        {
            transform.position += new Vector3(shakeSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x > startingPosition.x + shakeXBounds)
            {
                movingLeft = !movingLeft;
            }
        }
    }
}
