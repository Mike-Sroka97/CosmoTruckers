using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    bool spinning = false;
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!spinning)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotateWheel(-22.5f);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateWheel(22.5f);
            }
        }
    }

    private void RotateWheel(float rotationValue)
    {
        StartCoroutine(SpinWheel(rotationValue));
    }

    IEnumerator SpinWheel(float rotationValue)
    {
        spinning = true;
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        float currentDegree = 0;
        bool negative = rotationValue < 0;
        Vector3 currentRotation = transform.eulerAngles;

        while(currentDegree < MathF.Abs(rotationValue))
        {
            if(negative)
            {
                transform.Rotate(0, 0, -Time.deltaTime * speed);
            }
            else
            {
                transform.Rotate(0, 0, Time.deltaTime * speed);
            }

            foreach (RectTransform child in children)
            {
                if (!child.GetComponent<AttackUI>())
                {
                    child.rotation = Quaternion.identity;
                }
            }

            currentDegree += Time.deltaTime * speed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotation.z + rotationValue);
        foreach (RectTransform child in children)
        {
            if(!child.GetComponent<AttackUI>())
            {
                child.rotation = Quaternion.identity;
            }
        }
        spinning = false;
    }
}
