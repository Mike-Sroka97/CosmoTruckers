using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyEnemy : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float sendBackDistance;
    [HideInInspector] public bool DoneMoving = false;
    [HideInInspector] public float StartDelay;

    float currentTime = 0;
    float currentDistance = 0;
    bool delayOver = false;
    bool movingBack = false;
    float startingX;

    private void Start()
    {
        startingX = transform.position.x;
    }

    private void Update()
    {
        Movement();
        TrackTime();
    }

    private void Movement()
    {
        if(!DoneMoving && delayOver && !movingBack)
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void TrackTime()
    {
        if(!delayOver)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= StartDelay)
            {
                delayOver = true;
            }
        }
    }

    public void SendBack()
    {
        if(!movingBack)
        {
            StartCoroutine(SendBackCoroutine());
        }
    }

    IEnumerator SendBackCoroutine()
    {
        movingBack = true;
        while(currentDistance < sendBackDistance)
        {
            transform.position -= new Vector3(movementSpeed * Time.deltaTime * 2, 0, 0);
            currentDistance += movementSpeed * Time.deltaTime * 2;
            if(transform.position.x < startingX)
            {
                transform.position = new Vector3(startingX, transform.position.y, transform.position.z);
                currentDistance = sendBackDistance;
            }
            yield return new WaitForSeconds(0);
        }
        currentDistance = 0;
        movingBack = false;
    }
}
