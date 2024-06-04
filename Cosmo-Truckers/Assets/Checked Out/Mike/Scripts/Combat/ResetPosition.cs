using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform resetTransform;
    [SerializeField] Transform startTransform;
    [SerializeField] bool x = true;
    [SerializeField] bool negative = false;

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if(x)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if(!negative)
            {
                if (transform.position.x > resetTransform.position.x)
                    transform.position = startTransform.position;
            }
            else
            {
                if (transform.position.x < resetTransform.position.x)
                    transform.position = startTransform.position;
            }
        }
        else
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            if (!negative)
            {
                if (transform.position.y > resetTransform.position.y)
                    transform.position = startTransform.position;
            }
            else
            {
                if (transform.position.y < resetTransform.position.y)
                    transform.position = startTransform.position;
            }
        }
    }
}
