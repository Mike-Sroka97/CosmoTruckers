using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTINA : Player
{
    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject upAttackArea;

    [SerializeField] float jumpSpeedAccrual;
    [SerializeField] float jumpMaxHoldTime;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpGroundedDelay;

    [SerializeField] float hopForceModifier;
    [SerializeField] float raycastHopHelper;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;

    float currentJumpStrength;
    float currentJumpHoldTime = 0;
    float currentCoyoteTime;

    int layermask = 1 << 9; //ground
    Vector2 bottomLeft;
    Vector2 bottomRight;

    SpriteRenderer mySprite;
    Collider2D myCollider;

    float originalMoveSpeed;

    private void Start()
    {
        PlayerInitialize();

        originalMoveSpeed = moveSpeed;
        currentJumpStrength = 0;
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = transform.Find("Body").GetComponent<Collider2D>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
        SpecialMove();

        if(!IsGrounded(raycastHopHelper))
        {
            currentCoyoteTime += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9 && IsGrounded(.02f))
        {
            ShortHop();
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && IsGrounded(.02f))
        {
            ShortHop();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            transform.parent = null;
        }
    }

    public void ResetMoveSpeed() { moveSpeed = originalMoveSpeed; }
    public void SetMoveSpeed(float newSpeed) { moveSpeed = newSpeed; }
    public float GetMoveSpeed() { return moveSpeed; }
    public bool GetIsJumping() { return isJumping; }

    public override IEnumerator Damaged()
    {
        float damagedTime = 0;

        while (damagedTime < iFrameDuration)
        {
            mySprite.enabled = !mySprite.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                damaged = false;
                ShortHop();
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
        mySprite.enabled = true;
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack && !isJumping)
        {
            StartCoroutine(SafeTAttack(upAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && !isJumping)
        {
            StartCoroutine(SafeTAttack(horizontalAttackArea));
        }
    }

    IEnumerator SafeTAttack(GameObject attack)
    {
        canAttack = false;
        attack.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attack.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// SafeT's jump is based around charging it up for either a small or large jump
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
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space") && (IsGrounded(raycastHopHelper) || currentCoyoteTime < coyoteTime))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            myBody.AddForce(new Vector2(0, currentJumpStrength), ForceMode2D.Impulse);
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            StartCoroutine(JustJumped());
        }
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space"))
        {
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            StartCoroutine(JustJumped());
        }
    }

    IEnumerator JustJumped()
    {
        yield return new WaitForSeconds(jumpGroundedDelay);

        isJumping = false;
    }

    private void ShortHop()
    {
        if (IsGrounded(raycastHopHelper))
        {
            if (!isJumping)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0);
                myBody.AddForce(new Vector2(0, hopForceModifier), ForceMode2D.Impulse);
            }
            canJump = true;
            currentCoyoteTime = 0;
        }
        else
        {
            canJump = false;
        }
    }

    private bool IsGrounded(float distance)
    {
        bottomLeft = new Vector2(myCollider.bounds.min.x, myCollider.bounds.min.y);
        bottomRight = new Vector2(myCollider.bounds.max.x, myCollider.bounds.min.y);

        if (Physics2D.Raycast(bottomLeft, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(bottomRight, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask))
            return true;

        return false;
    }
    #endregion

    #region Movement
    /// <summary>
    /// Regular player movement with funny haha hops
    /// </summary>
    public void Movement()
    {
        if (!canMove || damaged) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);

            if (transform.rotation.eulerAngles.y == 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);

            if (transform.rotation.eulerAngles.y != 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
        }
        else if(!isJumping)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Jump cancel
    /// </summary>
    public void SpecialMove()
    {
        if(isJumping && Input.GetKeyDown(KeyCode.Mouse1))
        {
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            StartCoroutine(JustJumped());
        }
    }
    #endregion
}
