using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCoursePlatformMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveTime;
    [SerializeField] float moveDelay;
    [SerializeField] GameObject[] startMoveObjects;

    float currentTime = 0;
    bool trackTime = false;

    public void StartMove()
    {
        // TODO: Chance - delete this and use random objects instead please!
        if (startMoveObjects != null)
        {
            foreach (GameObject gO in startMoveObjects)
                gO.SetActive(true);
        }

        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime > moveDelay && currentTime < moveTime + moveDelay)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
