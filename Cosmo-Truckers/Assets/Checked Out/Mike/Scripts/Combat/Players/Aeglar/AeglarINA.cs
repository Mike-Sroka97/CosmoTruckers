using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveCD;

    [SerializeField] float dashSpeed;
    [SerializeField] float dashCD;
    [SerializeField] float dashDuration;
    [SerializeField] float dashSlow;

    [SerializeField] float jumpSpeed;

    bool canDash = true;
    bool canMove = true;
    bool canJump = true;
    bool dashing = false;
    PlayerCharacterINA INA;
    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && canDash && myBody.velocity.x < 0)
        {
            StartCoroutine(Dash(true));
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && canDash && myBody.velocity.x > 0)
        {
            StartCoroutine(Dash(false));
        }

        IEnumerator Dash(bool left)
        {
            dashing = true;
            canDash = false;
            canMove = false;

            myBody.velocity = Vector2.zero;

            if (left)
            {
                myBody.AddForce(new Vector2(-dashSpeed, 0));
            }
            else
            {
                myBody.AddForce(new Vector2(dashSpeed, 0));
            }

            float currentDashTime = 0;
            while(currentDashTime < dashDuration)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0);
                currentDashTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            myBody.velocity = new Vector2(myBody.velocity.x / dashSlow, myBody.velocity.y);

            yield return new WaitForSeconds(dashCD - currentDashTime);

            canMove = true;
            canDash = true;
        }
    }
    #endregion

    #region Jump
    /// <summary>
    /// Aeglar will not be able to move while jumping. He can still dash
    /// </summary>
    public void Jump()
    {
        if(Input.GetKeyDown("space") && canJump)
        {
            canJump = false;
            myBody.AddForce(new Vector2(0, jumpSpeed));
        }
    }
    #endregion

    #region Movement
    /// <summary>
    /// Aeglar's movement will cause short spurts of movements. There is an associated cooldown to this
    /// </summary>
    public void Movement()
    {
        if (!canMove) return;

        switch (Input.inputString)
        {
            case "a":
                StartCoroutine(Move(true));
                break;
            case "d":
                StartCoroutine(Move(false));
                break;
            default:
                break;
        }
    }

    IEnumerator Move(bool left)
    {
        canMove = false;

        if (left)
        {
            myBody.AddForce(new Vector2(-moveSpeed, 0));
        }
        else
        {
            myBody.AddForce(new Vector2(moveSpeed, 0));
        }

        yield return new WaitForSeconds(moveCD);

        if(!dashing)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
        canMove = true;
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>
    public void SpecialMove()
    {
        //not implementing this functionality from the interface
    }
    #endregion
}
