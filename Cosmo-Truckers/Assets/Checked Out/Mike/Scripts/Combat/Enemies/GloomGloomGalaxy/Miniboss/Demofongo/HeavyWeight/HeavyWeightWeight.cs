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
    [SerializeField] float startingGravity = 10f;
    Rigidbody2D myBody;
    Vector3 startingPosition;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        startingX = transform.localPosition.x;
        startingPosition = transform.localPosition;

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

        transform.localPosition = new Vector3(startingX, transform.localPosition.y, transform.localPosition.z);
        myBody.gravityScale = startingGravity;

        while(transform.localPosition.y > stopHeight)
        {
            yield return null;
        }

        myBody.velocity = Vector2.zero;
        transform.localPosition = new Vector3(transform.localPosition.x, stopHeight, transform.localPosition.z);
        myBody.gravityScale = 0;
        GameObject newShockwave = Instantiate(shockwave, shockwaveSpawn.position, shockwaveSpawn.rotation, transform.parent);

        yield return new WaitForSeconds(groundWaitTime);

        StartCoroutine(otherWeight.Shake());

        while (transform.localPosition != startingPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startingPosition, retractSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ShakeMe()
    {
        if(goingLeft)
        {
            transform.localPosition -= new Vector3(shakeSpeed * Time.deltaTime, 0, 0);

            if(transform.localPosition.x < startingX - shakeClamp)
            {
                goingLeft = !goingLeft;
            }
        }
        else
        {
            transform.localPosition += new Vector3(shakeSpeed * Time.deltaTime, 0, 0);

            if (transform.localPosition.x > startingX + shakeClamp)
            {
                goingLeft = !goingLeft;
            }
        }
    }
}
