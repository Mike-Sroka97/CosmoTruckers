using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBCircleEnemy : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float speed;
    [SerializeField] float transformSpeed;

    float angle;
    Rigidbody2D myBody;


    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CircularMotion();
    }

    private void CircularMotion()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        myBody.velocity = new Vector2(x, y);
        if (myBody.velocity.x > 0)
        {
            transform.position += new Vector3(transformSpeed * Time.deltaTime, 0, 0);
        }
    }
}
