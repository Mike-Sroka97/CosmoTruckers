using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject attackArea;

    [SerializeField] float jumpSpeedAccrual;
    [SerializeField] float jumpMaxHoldTime;

    [SerializeField] float hopForceModifier;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;

    float currentJumpStrength;
    float currentJumpHoldTime = 0;

    PlayerCharacterINA INA;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    private void Start()
    {
        currentJumpStrength = 0;
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if(!isJumping)
            {
                myBody.AddForce(new Vector2(0, hopForceModifier), ForceMode2D.Impulse);
            }
            canJump = true;
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
            StartCoroutine(SafeTAttack());
        }
    }

    IEnumerator SafeTAttack()
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
            canMove = false;
            canJump = false;
            isJumping = true;
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            if(currentJumpHoldTime > jumpMaxHoldTime) { currentJumpHoldTime = jumpMaxHoldTime; }
            currentJumpStrength = currentJumpHoldTime * jumpSpeedAccrual;
        }
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space"))
        {
            myBody.AddForce(new Vector2(0, currentJumpStrength), ForceMode2D.Impulse);
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            isJumping = false;
            canMove = true;
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

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y == 0 && !attackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y != 0 && !attackArea.activeInHierarchy)
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

    }
    #endregion
}
