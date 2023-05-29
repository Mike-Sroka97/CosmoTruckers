using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStempPlatform : MonoBehaviour
{
    SixfaceINA sixFace;

    private void Start()
    {
        sixFace = FindObjectOfType<SixfaceINA>();
    }

    private void Update()
    {
        if(!sixFace.Grounded())
        {
            Destroy(gameObject);
        }
    }
}
