using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSProjectile : MonoBehaviour
{
    [SerializeField] float initialDelay;
    [SerializeField] float speed;
    [SerializeField] float xSpawns;

    Vector3 startPosition;
    float startTime;
    float height;
    float distance;
    Transform player;
    bool movingRight;
    bool waiting = true;
    float currentTime = 0;
    PawnStar minigame;
    Transform mySpriteObject; 

    private void Start()
    {
        player = FindObjectOfType<SixfaceINA>().transform;
        minigame = FindObjectOfType<PawnStar>();
        mySpriteObject = GetComponentInChildren<Transform>();
    }

    private void RandomDistance()
    {
        float random = UnityEngine.Random.Range(0f, xSpawns);
        float randomBool = UnityEngine.Random.Range(0, 2); //coin flip bb
        if (randomBool == 0)
        {
            random = -random;
        }
        startPosition = new Vector3(random, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if(!waiting)
        {
            ParabolicMotion();
        }
        else
        {
            TrackTime();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            Debug.Log(minigame.PlayerDead);
        }
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= initialDelay)
        {
            waiting = false;
            startTime = Time.time;
            height = Mathf.Abs(player.position.y) + Mathf.Abs(startPosition.y); //offset from starting negative y value

            RandomDistance();

            //startPosition = transform.position;
            distance = Mathf.Abs(xSpawns) - Mathf.Abs(transform.position.x);

            if (player.position.x < startPosition.x)
            {
                movingRight = false;
            }
            else
            {
                movingRight = true;
            }
        }
    }

    private void ParabolicMotion()
    {
        float nextX;

        float normalizedTime = (Time.time - startTime) * speed / distance;

        //calculate angle
        float angle = normalizedTime * 180f;
        float yAngle = 0f; 

        if (!movingRight)
        {
            nextX = startPosition.x - normalizedTime * distance;
        }
        else
        {
            nextX = startPosition.x + normalizedTime * distance;
            yAngle = 180f; 
        }

        float nextY = startPosition.y + height * Mathf.Sin(normalizedTime * Mathf.PI);


        transform.position = new Vector3(nextX, nextY, transform.position.z);

        //Set Z to angle, y angle should be based on moving left or right
        mySpriteObject.localEulerAngles = new Vector3(mySpriteObject.localEulerAngles.x, yAngle, angle);

        if (transform.position.y < startPosition.y)
        {
            ResetParabola();
        }
    }

    private void ResetParabola()
    {
        startTime = Time.time;
        RandomDistance();

        RandomDistance();
        //distance = Mathf.Abs(xSpawns) - Mathf.Abs(transform.position.x);
        mySpriteObject.eulerAngles = new Vector3(mySpriteObject.eulerAngles.x, mySpriteObject.eulerAngles.y, 0f); //reset y rotation
        height = Mathf.Abs(player.position.y) + Mathf.Abs(startPosition.y); //offset from starting negative y value
        if (player.position.x < startPosition.x)
        {
            movingRight = false;
        }
        else
        {
            movingRight = true;
        }
    }
}
