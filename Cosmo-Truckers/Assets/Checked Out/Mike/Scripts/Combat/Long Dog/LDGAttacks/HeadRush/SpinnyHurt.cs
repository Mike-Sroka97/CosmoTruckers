using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyHurt : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] bool spinLeft = true;
    void Update()
    {
        if(spinLeft)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
        }
    }
}
