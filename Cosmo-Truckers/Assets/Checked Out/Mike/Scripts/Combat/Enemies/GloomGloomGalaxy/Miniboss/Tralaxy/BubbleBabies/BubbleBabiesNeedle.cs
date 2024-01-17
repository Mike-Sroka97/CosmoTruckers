using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesNeedle : MonoBehaviour
{
    [SerializeField] Transform[] needlePositions;
    [SerializeField] float moveTowardsSpeed;
    [SerializeField] float moveForwardSpeed;

    int currentPosition = 0;
    bool isMoving;
    bool canSwitchPositions = true;
    BubbleBabies minigame;
    MoveForward myMoveForward;

    public void Initialize()
    {
        minigame = FindObjectOfType<BubbleBabies>();
        myMoveForward = GetComponent<MoveForward>();
        Invoke("Fire", minigame.FireDelay);
        GetComponent<MoveForward>().ToggleClamps();
    }

    private void Fire()
    {
        canSwitchPositions = false;
        isMoving = true; 
        myMoveForward.MoveSpeed = moveForwardSpeed;
        StopAllCoroutines(); 
    }

    public void MoveMe(bool left = false)
    {
        if (isMoving && !canSwitchPositions)
            return;

        if (left && currentPosition != 0)
        {
            currentPosition--;
            StartCoroutine(MoveTowardsNextNode());
        }
        else if (!left && currentPosition != needlePositions.Length - 1)
        {
            currentPosition++;
            StartCoroutine(MoveTowardsNextNode());
        }
    }

    IEnumerator MoveTowardsNextNode()
    {
        isMoving = true;

        while(transform.position != needlePositions[currentPosition].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, needlePositions[currentPosition].position, moveTowardsSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
