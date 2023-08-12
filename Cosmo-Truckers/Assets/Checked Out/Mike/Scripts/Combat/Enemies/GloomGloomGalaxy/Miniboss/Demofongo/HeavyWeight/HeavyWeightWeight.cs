using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeightWeight : MonoBehaviour
{
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeClamp;
    [SerializeField] float groundWaitTime;
    [SerializeField] float retractSpeed;
    [SerializeField] HeavyWeightWeight otherWeight;
    [SerializeField] bool startingWeight;
    [SerializeField] Transform shockwaveSpawn;
    [SerializeField] GameObject shockwave;
    [SerializeField] float stopHeight;

    bool goingLeft = true;
    float startingX;
    float startingGravity;
    Rigidbody2D myBody;
    Vector3 startingPosition;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        startingGravity = myBody.gravityScale;
        myBody.gravityScale = 0;
        startingX = transform.position.x;
        startingPosition = transform.position;

        if (startingWeight)
            StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        float currentTime = 0;

        while(currentTime < shakeDuration)
        {
            ShakeMe();

            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(startingX, transform.position.y, transform.position.z);
        myBody.gravityScale = startingGravity;

        while(transform.position.y > stopHeight)
        {
            yield return null;
        }

        myBody.velocity = Vector2.zero;
        transform.position = new Vector3(transform.position.x, stopHeight, transform.position.z);
        myBody.gravityScale = 0;
        GameObject newShockwave = Instantiate(shockwave, shockwaveSpawn.position, shockwaveSpawn.rotation);

        yield return new WaitForSeconds(groundWaitTime);

        StartCoroutine(otherWeight.Shake());

        while (transform.position != startingPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, retractSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ShakeMe()
    {
        if(goingLeft)
        {
            transform.position -= new Vector3(shakeSpeed * Time.deltaTime, 0, 0);

            if(transform.position.x < startingX - shakeClamp)
            {
                goingLeft = !goingLeft;
            }
        }
        else
        {
            transform.position += new Vector3(shakeSpeed * Time.deltaTime, 0, 0);

            if (transform.position.x > startingX + shakeClamp)
            {
                goingLeft = !goingLeft;
            }
        }
    }
}
