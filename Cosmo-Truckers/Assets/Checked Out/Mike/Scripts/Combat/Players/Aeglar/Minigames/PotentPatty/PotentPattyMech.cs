using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPattyMech : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallVelocity;

    [SerializeField] float xClamp;
    [SerializeField] float yClamp;

    Rigidbody2D myBody;
    AeglarINA aeglar;
    Rigidbody2D aeglarBody;

    bool isJumping = false;
    bool isDashing = false;
    bool moveStarted = false;

    public void StartMove()
    {
        myBody = GetComponent<Rigidbody2D>();
        aeglar = FindObjectOfType<AeglarINA>();
        aeglarBody = aeglar.GetComponent<Rigidbody2D>();
        moveStarted = true;
    }

    private void Update()
    {
        if (!moveStarted)
            return;

        Dash();
        Jump();
        MoveMe();   
    }

    private void Dash()
    {
        if(!isDashing)
        {
            if (aeglar.DashingLeft)
            {
                StartCoroutine(DashSpeed(-dashSpeed));
            }
            else if(aeglar.DashingRight)
            {
                StartCoroutine(DashSpeed(dashSpeed));
            }
        }
        else
        {
            ClampCheck();
        }
    }

    private IEnumerator DashSpeed(float xVelocity)
    {
        isDashing = true;

        myBody.velocity = new Vector2(xVelocity, myBody.velocity.y);
        while(!aeglar.GetDashState())
        {
            yield return null;
        }

        isDashing = false;
    }

    private void Jump()
    {
        if(aeglar.DashingUp && !isJumping && transform.position.y < 0)
        {
            StartCoroutine(DashUp());
        }
        else if(!aeglar.DashingUp &&!isJumping)
        {
            if(transform.position.y > yClamp)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, fallVelocity);
            }
            else
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0);
            }
        }
    }

    private IEnumerator DashUp()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, jumpVelocity);
        isJumping = true;

        while (aeglar.DashingUp)
        {
            yield return null;
        }

        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        isJumping = false;
    }

    private void MoveMe()
    {
        if (isDashing)
            return;

        if(aeglarBody.velocity.x > 0 && transform.position.x < xClamp)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
        else if(aeglarBody.velocity.x < 0 && transform.position.x > -xClamp)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
    }

    private void ClampCheck()
    {
        if(transform.position.x > xClamp || transform.position.x < -xClamp)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
    }
}
