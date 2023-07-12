using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    private void Update()
    {
        RotateMe();
    }

    private void RotateMe()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }
}
