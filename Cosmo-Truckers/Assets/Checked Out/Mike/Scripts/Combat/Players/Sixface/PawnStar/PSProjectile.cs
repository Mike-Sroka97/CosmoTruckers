using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSProjectile : MonoBehaviour
{
    [SerializeField] float initialDelay;
    [SerializeField] float speed;
    [SerializeField] float xSpawns;
    [SerializeField] float firstTimeHeight = 2f; 

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

    bool firstTime = true; 

    private void Start()
    {
        player = FindObjectOfType<SixfaceINA>().transform;
        minigame = FindObjectOfType<PawnStar>();
        mySpriteObject = GetComponentInChildren<Transform>();
        startPosition = transform.localPosition; 
    }

    private void RandomDistance()
    {
        float random = UnityEngine.Random.Range(0f, xSpawns);
        float randomBool = UnityEngine.Random.Range(0, 2); //coin flip bb
        if (randomBool == 0)
        {
            random = -random;
        }
        startPosition = new Vector3(random, transform.localPosition.y, transform.localPosition.z);
    }

    private void Update()
    {
        if (!waiting)
        {
            ParabolicMotion();
        }
        else
        {
            TrackTime();
        }
    }

    private void TrackTime()
    {
        //Wait initial delay when minigame starts
        currentTime += Time.deltaTime;
        if (currentTime >= initialDelay)
        {
            waiting = false;
            startTime = Time.time;

            if (firstTime)
            {
                //Make sure it's high enough the first time to be visible
                height = firstTimeHeight + Mathf.Abs(startPosition.y);
                firstTime = false; 
            }
            else
            {
                height = Mathf.Abs(player.position.y) + Mathf.Abs(startPosition.y); //offset from starting negative y value
            }


            RandomDistance();

            startPosition = transform.localPosition;
            distance = Mathf.Abs(xSpawns) - Mathf.Abs(transform.localPosition.x);

            if (player.localPosition.x < startPosition.x)
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


        transform.localPosition = new Vector3(nextX, nextY, transform.localPosition.z);

        //Set Z to angle, y angle should be based on moving left or right
        mySpriteObject.localEulerAngles = new Vector3(mySpriteObject.localEulerAngles.x, yAngle, angle);

        if (transform.localPosition.y < startPosition.y)
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
        height = Mathf.Abs(player.localPosition.y) + Mathf.Abs(startPosition.y); //offset from starting negative y value
        if (player.localPosition.x < startPosition.x)
        {
            movingRight = false;
        }
        else
        {
            movingRight = true;
        }
    }
}
