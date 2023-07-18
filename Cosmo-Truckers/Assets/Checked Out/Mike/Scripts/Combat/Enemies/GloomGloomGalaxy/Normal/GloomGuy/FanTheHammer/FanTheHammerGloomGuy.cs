using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTheHammerGloomGuy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Collider2D[] myTriggers;
    [SerializeField] float xClamp;
    [SerializeField] float xVelocity;

    [Space(20)]
    [Header("Gun Stuff")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrel;
    [SerializeField] bool fan;
    [SerializeField] float fireDelay;
    [SerializeField] int numberOfTimesToFire = 1;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float fanDegreeIncrements;

    Rigidbody2D myBody;
    Transform gun;
    Player player;
    float currentTime = 0;

    bool goingLeft;
    bool moving = true;
    bool aiming = true;
    bool trackTime = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        gun = transform.Find("Gun");
        player = FindObjectOfType<Player>();
        float random = UnityEngine.Random.Range(-xClamp, xClamp);
        transform.position = new Vector3(random, transform.position.y, transform.position.z);

        if (transform.position.x <= 0)
        {
            goingLeft = true;
        }
        else
        {
            goingLeft = false;
        }
    }

    private void Update()
    {
        TrackTime();
        AimGun();
        MoveMe();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= fireDelay)
        {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        moving = false;
        trackTime = false;
        aiming = false;
        currentTime = 0;

        int currentBulletsFired = 0;

        while(currentBulletsFired < numberOfTimesToFire)
        {
            if(fan)
            {
                Fan();
            }
            else
            {
                GameObject tempBullet = Instantiate(bullet, barrel);
                tempBullet.transform.parent = null;
                tempBullet.transform.localScale = new Vector3(1, 1, 1);
            }

            yield return new WaitForSeconds(timeBetweenShots);

            currentBulletsFired++;
        }

        moving = true;
        trackTime = true;
        aiming = true;
    }

    private void Fan()
    {
        float startingRotation = fanDegreeIncrements * 6 / 2; //number of bullets divided by two

        for(int i = 0; i <= 6; i++)
        {
            GameObject tempBullet = Instantiate(bullet, barrel);
            tempBullet.transform.parent = null;
            tempBullet.transform.localScale = new Vector3(1, 1, 1);
            tempBullet.transform.Rotate(new Vector3(0, 0, startingRotation));

            startingRotation -= fanDegreeIncrements;
        }
    }

    private void AimGun()
    {
        if (!aiming)
            return;

        gun.up = -(player.transform.position - gun.position);
    }

    private void MoveMe()
    {
        if (!moving)
        {
            myBody.velocity = Vector2.zero;
            return;
        }

        if(goingLeft)
        {
            myBody.velocity = new Vector2(-xVelocity, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(xVelocity, myBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(Collider2D collider in myTriggers)
        {
            if (collider.gameObject == collision.gameObject)
            {
                goingLeft = !goingLeft;
            }
        }
    }
}
