using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkanatorCannon : MonoBehaviour
{
    [SerializeField] float spawnRate;
    [SerializeField] GameObject meatball;

    float currentTime;

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime > spawnRate)
        {
            currentTime = 0;
            Instantiate(meatball, transform);
        }
    }
}
