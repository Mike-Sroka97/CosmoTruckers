using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitationRaindrop : MonoBehaviour
{
    [SerializeField] Transform[] spawns;
    [SerializeField] float moveSpeed;
    [SerializeField] float minY;

    int lastRandom = -1;
    bool initialized = false;

    public void Initialize()
    {
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if(transform.localPosition.y < minY)
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
        transform.localPosition = spawns[random].localPosition;
    }
}
