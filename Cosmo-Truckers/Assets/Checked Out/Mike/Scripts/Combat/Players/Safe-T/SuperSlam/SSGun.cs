using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSGun : MonoBehaviour
{
    //Rotation Variables
    [SerializeField] float maxZRotation;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool rotatingRight;

    //Bullet variables
    [SerializeField] Transform barrel;
    [SerializeField] float fireDelay;
    [SerializeField] float volleyDelay;
    [SerializeField] int bulletsInVolley;
    [SerializeField] GameObject bullet;

    bool isFiring = false;    
    float currentTime = 0;
    int bulletsFired = 0;

    private void Update()
    {
        RotateMe();
        TrackTime();
    }

    private void TrackTime()
    {
        if(!isFiring)
        {
            currentTime += Time.deltaTime;
            if(currentTime > fireDelay)
            {
                currentTime = 0;
                isFiring = true;
                FireBullet();
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if(currentTime > volleyDelay)
            {
                currentTime = 0;
                FireBullet();

                if(bulletsFired >= bulletsInVolley)
                {
                    bulletsFired = 0;
                    isFiring = false;
                }
            }
        }
    }

    private void FireBullet()
    {
        bulletsFired++;
        Instantiate(bullet, barrel.position, transform.rotation);
    }

    private void RotateMe()
    {
        if(rotatingRight)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

            if(transform.eulerAngles.z > maxZRotation &&  transform.eulerAngles.z < 180)
            {
                transform.eulerAngles = new Vector3(0, 0, maxZRotation);
                rotatingRight = !rotatingRight;
            }
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));

            if (transform.eulerAngles.z < 360 - maxZRotation && transform.eulerAngles.z > 180)
            {
                transform.eulerAngles = new Vector3(0, 0, 360 - maxZRotation);
                rotatingRight = !rotatingRight;
            }
        }
    }
}
