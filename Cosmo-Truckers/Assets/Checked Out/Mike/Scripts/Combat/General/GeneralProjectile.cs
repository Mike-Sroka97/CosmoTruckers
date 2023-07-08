using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    const int xBound = 14;
    const int yBound = 12;

    private void Update()
    {
        MoveMe();
        BoundsCheck();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void BoundsCheck()
    {
        if(transform.position.x > xBound || transform.position.x < -xBound || transform.position.y > yBound || transform.position.y < -yBound)
        {
            Destroy(gameObject);
        }
    }
}
