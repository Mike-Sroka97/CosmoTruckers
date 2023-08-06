using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevastationCentralBall : MonoBehaviour
{
    [SerializeField] float upForce;
    [SerializeField] float yClamp;

    Vector3 startPos;
    Rigidbody2D myBody;

    private void Start()
    {
        startPos = transform.position;
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        TrackClamp();
    }

    private void TrackClamp()
    {
        if(transform.position.y <= yClamp)
        {
            myBody.velocity = Vector2.zero;
            transform.position = startPos;
            myBody.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);
        }
    }
}
