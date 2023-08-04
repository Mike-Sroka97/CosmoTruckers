using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_StarStruckSpawner : Augment
{
    [SerializeField] GameObject star;

    const float xClamp = 5.9f;
    const float minSpawn = 0.6f;
    const float maxSpawn = 0.9f;
    float spawnSpeed;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        spawnSpeed = -(StatusEffect * minSpawn);
        StartCoroutine(SpawnStar());
    }

    public override void StopEffect()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnStar()
    {
        yield return new WaitForSeconds(spawnSpeed);
        float randomX = Random.Range(-xClamp, xClamp);

        if (randomX >= 0)
        {
            GameObject tempStar = Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
            tempStar.transform.eulerAngles = new Vector3(0, 0, 225);
        }
        else
        {
            Instantiate(star, new Vector3(randomX, transform.position.y, transform.position.z), star.transform.rotation);
        }

        StartCoroutine(SpawnStar());
    }
}
