using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometkazeRing : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float minRandomSpeedDelay;
    [SerializeField] float maxRandomSpeedDelay;
    [SerializeField] bool rotatingLeft;

    float currentRandomDelay;
    float currentTime = 0;

    private void Start()
    {
        CalculateDelay();
    }

    private void Update()
    {
        RotateMe();
        TrackTime();
    }

    private void RotateMe()
    {
        if(rotatingLeft)
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
        }
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= currentRandomDelay)
        {
            currentTime = 0;
            rotatingLeft = !rotatingLeft;
            CalculateDelay();
        }
    }

    private void CalculateDelay()
    {
        currentRandomDelay = UnityEngine.Random.Range(minRandomSpeedDelay, maxRandomSpeedDelay);
    }
}
