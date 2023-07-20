using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStempPlatform : MonoBehaviour
{
    SixfaceINA sixFace;

    float timeToWait = 0.5f, timer; 

    private void Start()
    {
        sixFace = FindObjectOfType<SixfaceINA>();
    }

    private void Update()
    {
        //Cole changed this code to wait 0.5 seconds
        if (timer < timeToWait)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (!sixFace.Grounded())
            {
                Destroy(gameObject);
            }
        }
    }
}
