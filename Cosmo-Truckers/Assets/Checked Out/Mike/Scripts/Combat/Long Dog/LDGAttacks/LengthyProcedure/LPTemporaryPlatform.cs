using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPTemporaryPlatform : MonoBehaviour
{
    LongDogINA dog;

    private void Start()
    {
        dog = FindObjectOfType<LongDogINA>();        
    }

    private void Update()
    {
        if (dog.GetStretching())
        {
            Destroy(gameObject);
        }
    }
}
