using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGGun : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate;
    [SerializeField] GameObject barrel;
    [HideInInspector] public bool TrackingTime = false;
    [SerializeField] float flashingTime;
    [SerializeField] Color flashColor;
    [SerializeField] float timeBetweenFlashes;

    Transform player;
    float currentTime = 0;
    Color startingColor;
    SpriteRenderer myRenderer;
    FunGun minigame;
    bool nextGun = false;

    private void Start()
    {
        minigame = FindObjectOfType<FunGun>();
        player = FindObjectOfType<LongDogHead>().transform;
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
    }

    private void Update()
    {
        LookTowardsPlayer();
        TrackTime();
    }

    private void LookTowardsPlayer()
    {
        Vector3 playerDirection = player.position - transform.position;
        Quaternion newDirection = Quaternion.LookRotation(Vector3.forward, playerDirection);
        newDirection = new Quaternion(0, 0, newDirection.z, newDirection.w);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, newDirection, rotationSpeed * Time.deltaTime);
        transform.rotation = newRotation;
    }
    private void TrackTime()
    {
        if(TrackingTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= fireRate)
            {
                currentTime = 0;
                StartCoroutine(StartFire());
            }
            else if(currentTime >= fireRate / 2 && !nextGun)
            {
                nextGun = true;
                minigame.NextGun();
            }
        }
        else
        {
            currentTime = 0;
            nextGun = false;
        }
    }

    IEnumerator StartFire()
    {
        while(currentTime < flashingTime)
        {
            if(myRenderer.color == startingColor)
            {
                myRenderer.color = flashColor;
            }
            else
            {
                myRenderer.color = startingColor;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        myRenderer.color = startingColor;
        Instantiate(bullet, barrel.transform);
        currentTime = 0;
        TrackingTime = false;
    }
}
