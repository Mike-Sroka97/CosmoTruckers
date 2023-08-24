using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraviticSiphonProjectilePool : MonoBehaviour
{
    [SerializeField] float switchDelay;
    [SerializeField] float moveVelocity;

    Rigidbody2D myBody;
    bool goingRight = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(moveVelocity, 0);
        StartCoroutine(ChangeVelocity());
    }

    IEnumerator ChangeVelocity()
    {
        yield return new WaitForSeconds(switchDelay);

        goingRight = !goingRight;

        if (goingRight)
            myBody.velocity = new Vector2(moveVelocity, 0);
        else
            myBody.velocity = new Vector2(-moveVelocity, 0);

        StartCoroutine(ChangeVelocity());
    }
}
