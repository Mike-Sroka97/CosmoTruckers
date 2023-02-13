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
                RotateWheel(-18);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateWheel(18);
            }
        }
    }

    private void RotateWheel(int rotationValue)
    {
        StartCoroutine(SpinWheel(rotationValue));
    }

    IEnumerator SpinWheel(int rotationValue)
    {
        spinning = true;
        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        float currentDegree = 0;
        bool negative = rotationValue < 0;
        Quaternion currentRotation = transform.rotation;

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
                if (negative)
                {
                    child.Rotate(new Vector3(0, 0, Time.deltaTime * speed));
                }
                else
                {
                    child.Rotate(0, 0, -Time.deltaTime * speed);
                }
            }

            currentDegree += Time.deltaTime * speed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        spinning = false;
    }
}
