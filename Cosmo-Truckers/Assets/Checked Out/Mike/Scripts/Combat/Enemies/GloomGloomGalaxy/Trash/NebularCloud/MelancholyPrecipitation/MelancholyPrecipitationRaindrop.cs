using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitationRaindrop : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] float moveSpeed;
    [SerializeField] float minY;

    int lastRandom = -1;

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if(transform.position.y < minY)
        {
            DetermineSpawn();
        }
    }

    private void DetermineSpawn()
    {
        int random = UnityEngine.Random.Range(0, spawns.Length);
        while(random == lastRandom)
        {
            random = UnityEngine.Random.Range(0, spawns.Length);
        }
        lastRandom = random;
        transform.position = spawns[random].position;
    }
}
