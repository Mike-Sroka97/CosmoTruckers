using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float frequency = 1f;

    private void Update()
    {
        Oscillate();
    }

    private void Oscillate()
    {
        float t = (Mathf.Sin(Time.time * frequency) + 1.0f) * 0.5f;
        Vector3 newPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
        transform.position = newPosition;
    }
}
