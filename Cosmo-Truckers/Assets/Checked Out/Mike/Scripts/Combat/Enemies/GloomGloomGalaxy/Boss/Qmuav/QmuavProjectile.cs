using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavProjectile : MonoBehaviour
{
    [SerializeField] protected float maxVelocity = 8;
    [SerializeField] bool repeat = true;
    [SerializeField] bool checkClamps = true;

    Graviton myGraviton;
    Rigidbody2D myBody;
    Vector3 startPosition;

    const float xClamp = 6.5f;
    const float yClamp = 5f;

    protected virtual void Start()
    {
        myGraviton = GetComponent<Graviton>();
        myBody = GetComponent<Rigidbody2D>();
        startPosition = transform.localPosition;
    }

    protected virtual void Update()
    {
        CheckClamps();
        LimitVelocity();
    }

    protected void CheckClamps()
    {
        if (!checkClamps)
            return;

        if (transform.localPosition.x > xClamp
            || transform.localPosition.x < -xClamp
            || transform.localPosition.y > yClamp
            || transform.localPosition.y < -yClamp)
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
        if (!repeat)
            Destroy(gameObject);

        transform.localPosition = startPosition;
        myBody.velocity = Vector2.zero;
        myBody.AddForce(myGraviton.GetInitialVelocity(), ForceMode2D.Impulse);
    }
}
