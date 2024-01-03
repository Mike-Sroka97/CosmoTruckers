using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIronClock : MonoBehaviour
{
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float scoreWaitTime = 1f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;

    float rotationSpeed; //randomize
    float currentDegreesRotated = 0;
    float currentTime = 0;

    bool spinning = true;
    bool trackTime = false;
    public bool Activated = false;

    private void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        //direction correction
        rotationSpeed = -rotationSpeed;
    }

    private void Update()
    {
        TrackTime();
        RotateMe();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= scoreWaitTime)
        {
            GameObject bulletTemp = Instantiate(bullet, barrel);
            bulletTemp.transform.parent = null;
            bulletTemp.transform.localScale = new Vector3(1, 1, 1);
            bulletTemp.transform.position = barrel.position;
            trackTime = false;
        }
    }

    private void RotateMe()
    {
        if (!spinning)
            return;

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        currentDegreesRotated += rotationSpeed * Time.deltaTime;

        if(currentDegreesRotated <= -360)
        {
            spinning = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
            trackTime = true;
        }
    }

    public void Fire()
    {
        float scoreTime = 0;

        if(currentTime <= 0 || currentTime > scoreWaitTime)
        {
            scoreTime = 0;
        }
        else
        {
            scoreTime = currentTime;
        }

        //MATH
    }
}
