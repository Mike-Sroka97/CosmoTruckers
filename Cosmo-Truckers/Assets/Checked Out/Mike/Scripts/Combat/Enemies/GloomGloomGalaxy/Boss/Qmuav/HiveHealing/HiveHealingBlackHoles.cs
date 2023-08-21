using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveHealingBlackHoles : MonoBehaviour
{
    [SerializeField] Transform positionOne;
    [SerializeField] Transform positionTwo;
    [SerializeField] float moveTowardsSpeed;

    bool onPositionOne = true;

    public void ToggleMe()
    {
        if(onPositionOne)
            StartCoroutine(MoveMe(positionTwo));
        else
            StartCoroutine(MoveMe(positionOne));

        onPositionOne = !onPositionOne;
    }

    private IEnumerator MoveMe(Transform newPosition)
    {
        while(transform.position != newPosition.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition.position, moveTowardsSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
