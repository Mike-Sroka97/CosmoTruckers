using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
