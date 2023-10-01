using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTongueOrNotToTongueSpitter : MonoBehaviour
{
    [SerializeField] Transform spitter;
    [SerializeField] float lookTowardsSpeed;

    [SerializeField] float shootDelay;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;
    [SerializeField] Animator tongueAnimator;
    [SerializeField] AnimationClip tongueShoot; 

    Player player;
    Collider2D aggroRadius;
    bool isAggro = false;
    float currentShootTime = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        aggroRadius = GetComponent<Collider2D>();
    }

    private void Update()
    {
        TrackTime();
        TrackPlayer();
    }

    private void TrackTime()
    {
        if (!isAggro)
            return;

        currentShootTime += Time.deltaTime;

        if(currentShootTime >= shootDelay)
        {
            currentShootTime = 0;
            tongueAnimator.Play(tongueShoot.name);
            GameObject tempBullet = Instantiate(bullet, barrel);
            tempBullet.transform.parent = null;
            tempBullet.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void TrackPlayer()
    {
        if (!isAggro)
            return;

        //Vector3 tempEulerAngles = new Vector3(0, 0, spitter.transform.eulerAngles.z);
        //Vector3 tempPlayerEulerAngles = new Vector3(0, 0, player.transform.eulerAngles.z);
        //spitter.transform.eulerAngles = Vector3.RotateTowards(tempEulerAngles, tempPlayerEulerAngles, lookTowardsSpeed * Time.deltaTime, lookTowardsSpeed * Time.deltaTime);

        // Calculate the direction from the current object's position to the target's position
        Vector3 directionToTarget = player.transform.position - spitter.transform.position;

        // Calculate the angle between the forward direction of the object and the direction to the target
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Smoothly interpolate the rotation towards the target angle over time
        float currentAngle = Mathf.LerpAngle(spitter.transform.eulerAngles.z, targetAngle, lookTowardsSpeed * Time.deltaTime);

        // Apply the new rotation to the object
        spitter.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, currentAngle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isAggro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            currentShootTime = 0;
            isAggro = false;
        }
    }
}
