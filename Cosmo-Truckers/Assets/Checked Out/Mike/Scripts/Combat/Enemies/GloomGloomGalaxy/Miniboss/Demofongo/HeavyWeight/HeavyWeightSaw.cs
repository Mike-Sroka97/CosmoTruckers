using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeightSaw : MonoBehaviour
{
    [SerializeField] float resetClamp;
    [SerializeField] Transform resetPoint;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        MoveMe();
        TrackClamp();
    }

    private void MoveMe()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }

    private void TrackClamp()
    {
        if (Mathf.Abs(transform.localPosition.x) > Mathf.Abs(resetClamp))
        {
            transform.localPosition = resetPoint.localPosition;
        }
    }
}
