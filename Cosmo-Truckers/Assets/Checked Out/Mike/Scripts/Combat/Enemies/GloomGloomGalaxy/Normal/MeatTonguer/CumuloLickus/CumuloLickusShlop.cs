using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloLickusShlop : MonoBehaviour
{
    [SerializeField] float startDelay;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxHeight;

    float currentTime = 0;
    bool move = false;
    bool trackTime = true;

    private void Update()
    {
        TrackTime();
        MoveMe();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= startDelay)
        {
            trackTime = false;
            move = true;
        }
    }

    private void MoveMe()
    {
        if (!move)
            return;

        transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        if(transform.localPosition.y >= maxHeight)
        {
            move = false;
            transform.localPosition = new Vector3(0, maxHeight, 0);
        }
    }
}
