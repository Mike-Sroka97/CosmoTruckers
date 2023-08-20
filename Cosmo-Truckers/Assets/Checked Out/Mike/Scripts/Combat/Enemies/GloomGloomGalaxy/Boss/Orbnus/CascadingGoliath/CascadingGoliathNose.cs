using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathNose : MonoBehaviour
{
    [SerializeField] float minY;
    [SerializeField] float fallSpeed;

    private void Update()
    {
        if (transform.position.y <= minY)
            return;

        MoveMe();
    }

    private void MoveMe()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if(transform.position.y <= minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
    }
}
