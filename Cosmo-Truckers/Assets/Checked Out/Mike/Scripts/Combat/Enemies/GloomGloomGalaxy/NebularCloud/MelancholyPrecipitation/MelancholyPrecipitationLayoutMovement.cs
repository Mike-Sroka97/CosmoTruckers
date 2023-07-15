using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelancholyPrecipitationLayoutMovement : MonoBehaviour
{
    [SerializeField] float startDelay = 1f;
    [SerializeField] float moveSpeed;

    float currentTime = 0;


    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= startDelay)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
