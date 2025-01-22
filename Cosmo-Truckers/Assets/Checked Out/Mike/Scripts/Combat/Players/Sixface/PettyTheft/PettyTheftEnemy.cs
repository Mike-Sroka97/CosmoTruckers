using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PettyTheftEnemy : MonoBehaviour
{
    //bobbing variables
    [SerializeField] float distanceToFloat;
    [SerializeField] float floatSpeed;

    //horizontal motion variables
    [SerializeField] float distanceToSway;
    [SerializeField] float swaySpeed;

    //rotation motion variables
    [SerializeField] float distanceToRotate = 45f;
    [SerializeField] float rotationSpeed;

    //Money variables
    [SerializeField] GameObject Money;
    [HideInInspector] public bool onlyBob = false;
    [SerializeField] Collider2D[] collidersToDisable;
    [SerializeField] GameObject[] objectsToDisable;

    [SerializeField] bool movingUp = true;
    [SerializeField] bool movingRight = true;
    [SerializeField] bool rotatingRight = true;
    [SerializeField] GameObject deathParticle;
    [SerializeField] Transform deathParticleSpawn;
    float currentDistanceVertical = 0;
    float currentDistanceHorizontal = 0;
    Transform child;
    PettyTheft minigame;

    private void Start()
    {
        minigame = FindObjectOfType<PettyTheft>();
        child = GetComponentsInChildren<Transform>()[1]; //ignores parent
    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Handles enemy movement
    /// </summary>
    private void Move()
    {
        if(!onlyBob)
        {
            RotateBeam();
            Sway();
        }
        Bob();
    }

    /// <summary>
    /// Rotates hurt beam
    /// </summary>
    private void RotateBeam()
    {
        if(rotatingRight)
        {
            child.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
        else
        {
            child.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        }

        if(Mathf.Abs(child.eulerAngles.z) > distanceToRotate && Mathf.Abs(child.eulerAngles.z) < 360 - distanceToRotate)
        {
            if(child.eulerAngles.z > distanceToRotate && child.eulerAngles.z < 100)
            {
                child.eulerAngles = new Vector3(0, 0, distanceToRotate);
            }
            else
            {
                child.eulerAngles = new Vector3(0, 0, 360 - distanceToRotate);
            }

            rotatingRight = !rotatingRight;
        }
    }

    /// <summary>
    /// Oscillates enemy left to right and vice versa
    /// </summary>
    private void Sway()
    {
        if (movingRight)
        {
            transform.position += new Vector3(swaySpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(swaySpeed * Time.deltaTime, 0, 0);
        }

        currentDistanceHorizontal += swaySpeed * Time.deltaTime;
        if (currentDistanceHorizontal > distanceToSway)
        {
            currentDistanceHorizontal = 0;
            movingRight = !movingRight;
        }
    }

    /// <summary>
    /// Bobs the enemy up and down
    /// </summary>
    private void Bob()
    {
        if(movingUp)
        {
            transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.position -= new Vector3(0, floatSpeed * Time.deltaTime, 0);
        }

        currentDistanceVertical += floatSpeed * Time.deltaTime;
        if (currentDistanceVertical > distanceToFloat)
        {
            currentDistanceVertical = 0;
            movingUp = !movingUp;
        }
    }

    /// <summary>
    /// What happens when the enemy takes damage
    /// </summary>
    public void Hurt()
    {
        foreach(Collider2D collider in collidersToDisable)
        {
            collider.enabled = false;
        }
        foreach(GameObject _object in objectsToDisable)
        {
            _object.SetActive(false);
        }

        Instantiate(deathParticle, deathParticleSpawn.position, Quaternion.identity, minigame.transform); 

        minigame.Score++;
        Money.SetActive(true);
        onlyBob = true;

        if(minigame.Score >= 4)
        {
            minigame.ActivateMoney();
        }
    }

    public void ActivateLights()
    {
        Light2D[] lights = GetComponentsInChildren<Light2D>();

        foreach (Light2D light in lights)
            light.enabled = true;
    }
}
