using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFuryBall : MonoBehaviour
{
    [SerializeField] bool goingUp;
    [SerializeField] float moveSpeed;

    const float yClamp = 4.5f;
    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(0, moveSpeed);
    }

    private void Update()
    {
        TrackClamp();
    }

    private void TrackClamp()
    {
        if(goingUp)
        {
            if(transform.localPosition.y >= yClamp)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -yClamp, transform.localPosition.z);
            }
        }
        else
        {
            if (transform.localPosition.y <= -yClamp)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, yClamp, transform.localPosition.z);
            }
        }
    }
}
