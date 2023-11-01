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

    //Laser variables
    [SerializeField] Transform laser; 
    [SerializeField] Transform barrel;
    [SerializeField] float laserResizeTime; 
    [SerializeField] float chargeTime;
    [SerializeField] GameObject chargeParticle;
    [SerializeField] AnimationClip fireCannon;

    ParticleUpdater myParticleUpdater; 
    Animator myAnimator;
    bool isFiring = false;
    float growTime = 0;  
    float shrinkTime;
    Vector3 zeroScale;
    Vector3 fullScale;
    Vector3 currentScale; 
    [HideInInspector] public bool trackTime;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myParticleUpdater = GetComponent<ParticleUpdater>();
        myParticleUpdater.SetParticleState(false);

        //Set scaling
        zeroScale = new Vector3(0, laser.localScale.y, laser.localScale.z);
        fullScale = new Vector3(1, laser.localScale.y, laser.localScale.z);
        currentScale = zeroScale;

        //Set this so it doesn't shrink during the beginning of the minigame
        shrinkTime = laserResizeTime; 
    }

    private void Update()
    {
        if (!trackTime)
            return;

        RotateMe();
        TrackLaser(); 
    }

    private void TrackLaser()
    {
        laser.localScale = currentScale; 

        if (isFiring)
        {
            shrinkTime = 0;

            if (growTime < laserResizeTime)
            {
                growTime += Time.deltaTime;
                currentScale = Vector3.Lerp(zeroScale, fullScale, growTime / laserResizeTime);
            }
            else 
            {
                currentScale = fullScale;
            }
        }
        else
        {
            growTime = 0;

            if (shrinkTime < laserResizeTime)
            {
                shrinkTime += Time.deltaTime;
                currentScale = Vector3.Lerp(fullScale, zeroScale, growTime / laserResizeTime);
            }
            else
            {
                currentScale = zeroScale;
            }
        }
    }

    public void SetLaserState(bool laserGrow)
    {
        if (laserGrow)
        {
            StartCoroutine(StartLaser());
            myParticleUpdater.SetParticleState(true);
        }
        else
        {
            //This check is for the beginning of the minigame
            if (shrinkTime > 0)
            {
                currentScale = zeroScale; 
            }

            isFiring = false;
            myParticleUpdater.SetParticleState(false);
        }
    }

    private IEnumerator StartLaser()
    {
        Instantiate(chargeParticle, barrel);

        yield return new WaitForSeconds(chargeTime);

        myAnimator.Play(fireCannon.name);
        currentScale = zeroScale;
        isFiring = true; 
    }

    private void RotateMe()
    {
        if(rotatingRight)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));

            if (transform.localEulerAngles.z > maxZRotation && transform.localEulerAngles.z < 180)
            {
                transform.localEulerAngles = new Vector3(0, 0, maxZRotation);
                rotatingRight = !rotatingRight;
            }
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));

            if (transform.localEulerAngles.z < 360 - maxZRotation && transform.localEulerAngles.z > 180)
            {
                transform.localEulerAngles = new Vector3(0, 0, 360 - maxZRotation);
                rotatingRight = !rotatingRight;
            }
        }
    }
}
