using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    //All these variables will need to pull from save data at some point to see how many attacks the player has
    const float radius = 40f;
    int numberOfAttacks = 16;
    float rotationDistance;

    bool spinning = false;
    RectTransform[] children;

    private void Start()
    {        
        children = GetComponentsInChildren<RectTransform>();
        float angle = 0f;
        rotationDistance = 360f / numberOfAttacks;
        float x;
        float y;

        for(int i = 0; i < numberOfAttacks; i++)
        {
            x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            children[i + 1].transform.localPosition = new Vector3(x, y, 0);
            angle += rotationDistance;
        }
    }
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
                RotateWheel(-rotationDistance);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateWheel(rotationDistance);
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
