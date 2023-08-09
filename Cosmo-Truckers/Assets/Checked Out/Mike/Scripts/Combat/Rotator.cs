using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotateSpeed;

    private void Update()
    {
        RotateMe();
    }

    protected void RotateMe()
    {
        transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));
    }
}
