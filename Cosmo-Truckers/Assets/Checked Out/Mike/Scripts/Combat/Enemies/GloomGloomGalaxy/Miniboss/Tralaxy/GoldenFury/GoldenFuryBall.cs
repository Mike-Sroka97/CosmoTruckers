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
            if(transform.position.y >= yClamp)
            {
                transform.position = new Vector3(transform.position.x, -yClamp, transform.position.z);
            }
        }
        else
        {
            if (transform.position.y <= -yClamp)
            {
                transform.position = new Vector3(transform.position.x, yClamp, transform.position.z);
            }
        }
    }
}
