using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixfaceINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject attackArea;

    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMaxHoldTime;

    [SerializeField] float hoverVelocityYMax;
    [SerializeField] float hoverGravityModifier;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canHover = true;

    float currentJumpStrength;
    float currentJumpHoldTime = 0;

    PlayerCharacterINA INA;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    private void Start()
    {
        currentJumpStrength = jumpSpeed;
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
        SpecialMove();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
            canHover = false;
            currentJumpHoldTime = 0;
            currentJumpStrength = jumpSpeed;
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
        }
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(SixFaceAttack());
        }
    }

    IEnumerator SixFaceAttack()
    {
        canAttack = false;
        attackArea.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackArea.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Aeglar will not be able to move while jumping. He can still dash
    /// </summary>
    public void Jump()
    {
        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.AddForce(new Vector2(0, jumpSpeed));
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            currentJumpStrength = jumpSpeed * ((jumpMaxHoldTime - currentJumpHoldTime) / jumpMaxHoldTime);
            myBody.AddForce(new Vector2(0, currentJumpStrength));
        }
        else if (isJumping)
        {
            isJumping = false;
            canHover = true;
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

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) && !attackArea.activeInHierarchy)
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !attackArea.activeInHierarchy)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>
    public void SpecialMove()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && canHover)
        {
            myBody.gravityScale = hoverGravityModifier;
            if(myBody.velocity.y < hoverVelocityYMax)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, hoverVelocityYMax);
            }
        }
        else
        {
            myBody.gravityScale = 1;
        }
    }
    #endregion
}
