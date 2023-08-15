using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholdDeathDross : MonoBehaviour
{
    [SerializeField] float flipMin;
    [SerializeField] float flipMax;
    [SerializeField] float rotatorSpeed = 50f;
    [SerializeField] float rotatorDelay;

    Rotator myRotator;

    private void Start()
    {
        myRotator = GetComponent<Rotator>();
        Invoke("StartRotation", rotatorDelay);
    }

    private void StartRotation()
    {
        myRotator.RotateSpeed = rotatorSpeed;
        StartCoroutine(FlipRotatorSpeed());
    }

    IEnumerator FlipRotatorSpeed()
    {
        float randomTime = Random.Range(flipMin, flipMax);

        yield return new WaitForSeconds(randomTime);

        myRotator.RotateSpeed = -myRotator.RotateSpeed;

        StartCoroutine(FlipRotatorSpeed());
    }
}
