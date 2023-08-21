using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavProjectile : MonoBehaviour
{
    [SerializeField] float maxVelocity = 8;

    Graviton myGraviton;
    Rigidbody2D myBody;
    Vector3 startPosition;

    const float xClamp = 6.5f;
    const float yClamp = 5f;

    private void Start()
    {
        myGraviton = GetComponent<Graviton>();
        myBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        CheckClamps();
        LimitVelocity();
    }

    private void CheckClamps()
    {
        if (transform.position.x > xClamp
            || transform.position.x < -xClamp
            || transform.position.y > yClamp
            || transform.position.y < -yClamp)
        {
            ResetPosition();
        }
    }

    private void LimitVelocity()
    {
        if (myBody.velocity.x > maxVelocity)
            myBody.velocity = new Vector2(maxVelocity, myBody.velocity.y);
        if (myBody.velocity.y > maxVelocity)
            myBody.velocity = new Vector2(myBody.velocity.x, maxVelocity);
        if (myBody.velocity.x < -maxVelocity)
            myBody.velocity = new Vector2(-maxVelocity, myBody.velocity.y);
        if (myBody.velocity.y < -maxVelocity)
            myBody.velocity = new Vector2(myBody.velocity.x, -maxVelocity);
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        myBody.velocity = Vector2.zero;
        myBody.AddForce(myGraviton.GetInitialVelocity(), ForceMode2D.Impulse);
    }
}
