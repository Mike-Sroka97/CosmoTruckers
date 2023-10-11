using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float bounds;
    [SerializeField] bool movingUp;
    [SerializeField] bool y = true;

    float startingPosition;

    private void Start()
    {
        if (y)
            startingPosition = transform.localPosition.y;
        else
            startingPosition = transform.localPosition.x;
    }

    private void Update()
    {
        if(y)
        {
            if(movingUp)
            {
                transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
                if (transform.localPosition.y > startingPosition + bounds)
                    movingUp = !movingUp;
            }
            else
            {
                transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
                if (transform.localPosition.y < startingPosition - bounds)
                    movingUp = !movingUp;
            }
        }
        else
        {
            if (movingUp)
            {
                transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
                if (transform.localPosition.x > startingPosition + bounds)
                    movingUp = !movingUp;
            }
            else
            {
                transform.localPosition -= new Vector3(speed * Time.deltaTime, 0, 0);
                if (transform.localPosition.x < startingPosition - bounds)
                    movingUp = !movingUp;
            }
        }
    }
}
