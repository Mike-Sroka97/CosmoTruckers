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

        while(currentDegree < MathF.Abs(rotationValue))
        {
            if(negative)
            {
                transform.parent.Rotate(0, 0, -Time.deltaTime * speed);
            }
            else
            {
                transform.parent.Rotate(0, 0, Time.deltaTime * speed);
            }

            //foreach (RectTransform child in children)
            //{
            //    child.rotation = new Quaternion(child.rotation.x, child.rotation.y, 0, child.rotation.z);
            //}

            currentDegree += Time.deltaTime * speed;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        spinning = false;
    }
}
