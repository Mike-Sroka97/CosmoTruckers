using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmAndRepulseQmuav : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] float distanceToTravel;
    [SerializeField] float travelSpeed;
    [SerializeField] float sideChangeDelay;
    [SerializeField] bool randomStart = true;
    [SerializeField] bool gs = false;

    bool left = false;
    GalaxyBusterSpawner galaxyBusterSpawner;
    private void Start()
    {
        galaxyBusterSpawner = GetComponentInChildren<GalaxyBusterSpawner>();

        if(randomStart)
        {
            int coinFlip = Random.Range(0, 2);
            if (coinFlip == 1)
                left = true;
        }

        StartCoroutine(MoveMe());
    }

    IEnumerator MoveMe()
    {
        if(left)
        {
            transform.position = spawns[0].position;
            while(transform.position.x <= spawns[0].position.x + distanceToTravel)
            {
                transform.position += new Vector3(travelSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.position = new Vector3(spawns[0].position.x + distanceToTravel, transform.position.y, transform.position.z);

            if (galaxyBusterSpawner)
                galaxyBusterSpawner.ShootProjectile(true);
        }
        else
        {
            transform.position = spawns[1].position;
            while (transform.position.x >= spawns[1].position.x - distanceToTravel)
            {
                transform.position -= new Vector3(travelSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.position = new Vector3(spawns[1].position.x - distanceToTravel, transform.position.y, transform.position.z);

            if (galaxyBusterSpawner)
                galaxyBusterSpawner.ShootProjectile(false);
        }

        yield return new WaitForSeconds(sideChangeDelay);

        if (left)
        {
            while (transform.position.x >= spawns[0].position.x)
            {
                transform.position -= new Vector3(travelSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.position = spawns[0].position;

            if(gs)
            {
                StartCoroutine(FindObjectOfType<GraviticSiphonProjectilePool>().ChangeVelocity());
            }
        }
        else
        {
            while (transform.position.x <= spawns[1].position.x)
            {
                transform.position += new Vector3(travelSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.position = spawns[1].position;

            if (gs)
            {
                StartCoroutine(FindObjectOfType<GraviticSiphonProjectilePool>().ChangeVelocity());
            }
        }

        left = !left;
        StartCoroutine(MoveMe());
    }
}
