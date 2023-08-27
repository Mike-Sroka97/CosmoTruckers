using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraviticSiphonProjectilePool : MonoBehaviour
{
    [SerializeField] float switchDelay;
    [SerializeField] float moveVelocity;
    [SerializeField] float transitionSpeed;

    Rigidbody2D myBody;
    bool goingRight = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(moveVelocity, 0);
        StartCoroutine(ChangeVelocity());
    }

    public IEnumerator ChangeVelocity()
    {
        if (goingRight)
        {
            while(myBody.velocity.x < moveVelocity)
            {
                myBody.velocity += new Vector2(transitionSpeed * Time.deltaTime, 0);
                yield return null;
            }
            myBody.velocity = new Vector2(moveVelocity, 0);
        }

        else
        {
            while (myBody.velocity.x > -moveVelocity)
            {
                myBody.velocity -= new Vector2(transitionSpeed * Time.deltaTime, 0);
                yield return null;
            }
            myBody.velocity = new Vector2(-moveVelocity, 0);
        }

        goingRight = !goingRight;
    }
}
