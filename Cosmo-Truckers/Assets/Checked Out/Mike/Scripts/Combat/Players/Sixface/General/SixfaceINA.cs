using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixfaceINA : Player
{
    //Attack variables
    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] float maxYvelocity = 4;
    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject downAttackArea;
    [SerializeField] GameObject upAttackArea;

    //Jump variables
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMaxHoldTime;
    [SerializeField] float pogoStrength;
    [SerializeField] float coyoteTime;

    //Hover variables
    [SerializeField] float hoverVelocityYMax;
    [SerializeField] float hoverGravityModifier;

    [HideInInspector] public bool IsHovering = false;
    bool canMove = true;
    bool canJump = true;
    bool isJumping = true;
    bool canAttack = true;
    bool canHover = true;

    float currentJumpHoldTime = 0;
    float currentCoyoteTime = 0;

    SpriteRenderer myRenderer;
    Collider2D myCollider;
    int layermask = 1 << 9;
    float startingGravity;

    const float distance = 0.05f;
    Vector2 bottomLeft;
    Vector2 bottomRight;

    private void Start()
    {
        PlayerInitialize();

        myBody = GetComponent<Rigidbody2D>();
        startingGravity = myBody.gravityScale;
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponentInChildren<Collider2D>(); //ignores parent
    }

    private void Update()
    {
        if(!damaged)
        {
            Attack();
            Movement();
            Jump();
            SpecialMove();
        }
    }

    private void IsGrounded()
    {
        bottomLeft = new Vector2(myCollider.bounds.min.x, myCollider.bounds.min.y);
        bottomRight = new Vector2(myCollider.bounds.max.x, myCollider.bounds.min.y);

        if (Physics2D.Raycast(bottomLeft, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(bottomRight, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask))
        {
            canJump = true;
            canHover = false;
            currentJumpHoldTime = 0;
            currentCoyoteTime = 0;
        }
        else if (currentCoyoteTime > coyoteTime)
        {
            canJump = false;
            canHover = true;
        }
        else
        {
            currentCoyoteTime += Time.deltaTime;
        }
    }

    public bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + .05f, layermask);
    }

    public override IEnumerator Damaged()
    {
        float damagedTime = 0;
        myBody.velocity = Vector2.zero;

        while(damagedTime < iFrameDuration)
        {
            myRenderer.enabled = !myRenderer.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if(damagedTime > damagedDuration)
            {
                damaged = false;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
        myRenderer.enabled = true;
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack)
        {
            StartCoroutine(SixFaceAttack(upAttackArea));
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.S) && canAttack && (currentJumpHoldTime != 0 || canHover))
        {
            StartCoroutine(SixFaceAttack(downAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(SixFaceAttack(horizontalAttackArea));
        }
    }

    IEnumerator SixFaceAttack(GameObject attack)
    {
        canAttack = false;
        attack.SetActive(true);
        attack.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attack.GetComponent<Collider2D>().enabled = false;
        attack.SetActive(false);
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
        IsGrounded();

        if(myBody.velocity.y > maxYvelocity)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, maxYvelocity);
        }

        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);
        }
        else if (isJumping)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * hoverGravityModifier);
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

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) && !horizontalAttackArea.activeInHierarchy)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !horizontalAttackArea.activeInHierarchy)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
    }

    public void Pogo()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        myBody.AddForce(new Vector2(0, pogoStrength));
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
            IsHovering = true;
            myBody.gravityScale = hoverGravityModifier;
            if(myBody.velocity.y < hoverVelocityYMax)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, hoverVelocityYMax);
            }
        }
        else
        {
            IsHovering = false;
            myBody.gravityScale = startingGravity;
        }
    }
    #endregion
}
