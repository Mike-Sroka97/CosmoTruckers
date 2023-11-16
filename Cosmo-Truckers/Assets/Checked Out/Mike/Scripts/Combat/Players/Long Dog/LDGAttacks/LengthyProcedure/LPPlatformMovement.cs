using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPPlatformMovement : MonoBehaviour
{
    [SerializeField] Transform pointOne;
    [SerializeField] Transform pointTwo;
    [SerializeField] float movementSpeed;

    bool onPointOne = true;
    Vector3 startingPos;
    Vector3 secondPos;
    LengthyProcedure minigame;

    private void Start()
    {
        minigame = FindObjectOfType<LengthyProcedure>();
    }

    public void StartMove()
    {
        startingPos = pointOne.position;
        secondPos = pointTwo.position;
    }

    public void Move()
    {
        StopAllCoroutines();
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        if(onPointOne)
        {
            onPointOne = false;
            while (Vector2.Distance(transform.position, secondPos) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, secondPos, movementSpeed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            transform.position = pointTwo.position;
        }
        else
        {
            onPointOne = true;
            while (Vector2.Distance(transform.position, startingPos) > 0.001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, startingPos, movementSpeed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            transform.position = pointOne.position;
        }
    }
}
