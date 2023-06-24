using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCoursePlatformMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveTime;
    [SerializeField] float moveDelay;

    float currentTime = 0;

    private void Start()
    {
        FindObjectOfType<AeglarINA>().transform.parent = transform;
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime > moveDelay && currentTime < moveTime + moveDelay)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
