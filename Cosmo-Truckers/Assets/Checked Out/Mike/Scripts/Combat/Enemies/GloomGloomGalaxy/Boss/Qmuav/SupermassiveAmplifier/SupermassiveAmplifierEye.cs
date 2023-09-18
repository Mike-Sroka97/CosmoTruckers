using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifierEye : MonoBehaviour
{
    [SerializeField] float fireDelay;
    [SerializeField] float rotateSpeed;

    //Firing variables
    [SerializeField] Transform pupil;
    [SerializeField] GameObject eyeProjectile;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int numberOfShots;
    [SerializeField] float maxStartingVelocity;

    [SerializeField] AnimationClip idle, ampEyeAnimation;

    Player currentTarget;
    Player[] players;
    bool trackTime = true;
    bool firing = false;
    float currentTime = 0;
    Animator myAnimator; 

    private void Start()
    {
        players = FindObjectsOfType<Player>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        LookAtPlayer();
        TrackTime();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= fireDelay)
        {
            trackTime = false;
            firing = true;
            currentTime = 0;
            StartCoroutine(Fire());
        }
    }

    private void LookAtPlayer()
    {
        if (firing)
            return;

        CalculatePlayerDistances();

        float angle = Mathf.Atan2(currentTarget.transform.position.y - transform.position.y, currentTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void CalculatePlayerDistances()
    {
        float closestDistance = 100;

        foreach (Player player in players)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < closestDistance)
            {
                currentTarget = player;
                closestDistance = Vector2.Distance(transform.position, player.transform.position);
            }
        }
    }

    IEnumerator Fire()
    {
        myAnimator.Play(ampEyeAnimation.name); 

        for(int i = 0; i < numberOfShots; i++)
        {
            float startingXvelocity = DetermineVelocity(true);
            float startingYvelocity = DetermineVelocity(false);

            Graviton newGraviton = Instantiate(eyeProjectile, pupil.position, Quaternion.identity).GetComponent<Graviton>();
            newGraviton.SetInitialVelocity(new Vector3(startingXvelocity, startingYvelocity, 0));
            newGraviton.enabled = true;

            yield return new WaitForSeconds(timeBetweenShots);
        }

        trackTime = true;
        currentTime = 0;
        firing = false;
        myAnimator.Play(idle.name);
    }

    private float DetermineVelocity(bool x)
    {
        float zEuler = transform.eulerAngles.z;

        if(x)
        {
            if(zEuler >= 270 && zEuler <= 360)
            {
                return Mathf.Abs(zEuler) / 360 * maxStartingVelocity;
            }
            else if(zEuler >= 0 && zEuler <= 90)
            {
                return Mathf.Abs(zEuler) / 90 * maxStartingVelocity;
            }
            else if(zEuler >= 90 && zEuler <= 180)
            {
                return -(Mathf.Abs(zEuler) / 180 * maxStartingVelocity);
            }
            else if(zEuler >= 180 && zEuler <= 270)
            {
                return -(Mathf.Abs(zEuler) / 270 * maxStartingVelocity);
            }
        }
        else
        {
            if (zEuler >= 270 && zEuler <= 360)
            {
                return -(360 / Mathf.Abs(zEuler) * maxStartingVelocity);
            }
            else if (zEuler >= 0 && zEuler <= 90)
            {
                return (90 / Mathf.Abs(zEuler) * maxStartingVelocity);
            }
            else if (zEuler >= 90 && zEuler <= 180)
            {
                return (180 / Mathf.Abs(zEuler) * maxStartingVelocity);
            }
            else if (zEuler >= 180 && zEuler <= 270)
            {
                return -(270 / Mathf.Abs(zEuler) * maxStartingVelocity);
            }
        }

        return 0;
    }
}
