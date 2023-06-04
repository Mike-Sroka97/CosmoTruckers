using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPnoteMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }
}
