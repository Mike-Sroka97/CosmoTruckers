using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORBFaceChunk : MonoBehaviour
{
    [SerializeField] Material hurtMaterial;
    [SerializeField] float shakeXBounds;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeSpeed;

    [SerializeField] float fallSpeed;
    [SerializeField] float yLimit;
    [SerializeField] float rotateSpeed = 0f; 

    SpriteRenderer mySpriteRenderer;
    bool movingLeft = true;
    float currentTime = 0;
    bool trackTime = false;
    bool falling;
    Collider2D myCollider;
    Vector3 startingPosition;

    void Start()
    {
        try
        {
            mySpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            myCollider = transform.GetChild(0).GetComponent<Collider2D>();
        }
        catch
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            myCollider = GetComponent<Collider2D>();
        }

        myCollider.enabled = false; 
        startingPosition = transform.position;
    }

    public void StartToFall()
    {
        trackTime = true;
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

        mySpriteRenderer.material = hurtMaterial;
        myCollider.enabled = true;
        transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);

        if (transform.position.y < yLimit)
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

        if (currentTime >= shakeDuration)
        {
            falling = true;
            trackTime = false;

            if (rotateSpeed > 0)
            {
                Rotator myRotator = gameObject.AddComponent<Rotator>();
                myRotator.RotateSpeed = rotateSpeed;
            }
        }
    }

    private void Shake()
    {
        if (movingLeft)
        {
            transform.position -= new Vector3(shakeSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < startingPosition.x - shakeXBounds)
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
