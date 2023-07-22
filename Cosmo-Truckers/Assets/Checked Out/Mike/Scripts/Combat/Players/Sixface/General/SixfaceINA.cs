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

    //Animation Stuff
    [Space(20)]
    [Header("0 - Smug, 1 - Question, 2 - Dizzy, 3 - Money, 4 - Sad, 5 - Hype")]
    [SerializeField] AnimationClip[] sixFaceFaces;
    [SerializeField] AnimationClip downAttack;
    [SerializeField] AnimationClip upAttack;
    [SerializeField] AnimationClip horizontalAttack;
    [SerializeField] AnimationClip changeFace;
    [SerializeField] AnimationClip hover;
    [SerializeField] AnimationClip hurt;
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip jump;
    [SerializeField] AnimationClip move;
    [SerializeField] float switchFaceDuration;

    [HideInInspector] public bool IsHovering = false;
    bool canMove = true;
    bool canJump = true;
    bool isJumping = true;
    bool canAttack = true;
    bool canHover = true;

    float currentJumpHoldTime = 0;
    float currentCoyoteTime = 0;

    Animator faceAnimator;
    Animator bodyAnimator;
    PlayerAnimator playerAnimator;
    Collider2D myCollider;
    int layermask = 1 << 9;
    float startingGravity;

    const float distance = 0.05f;
    Vector2 bottomLeft;
    Vector2 bottomRight;

    private void Start()
    {
        PlayerInitialize();

        faceAnimator = transform.Find("SixFaceFace").GetComponent<Animator>();
        bodyAnimator = transform.Find("SixFaceBody").GetComponent<Animator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        myBody = GetComponent<Rigidbody2D>();
        startingGravity = myBody.gravityScale;
        myCollider = GetComponentInChildren<Collider2D>(); //ignores parent
    }

    private void Update()
    {
        UpdateOutline();
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
            if(!damaged)
            {
                canJump = true;
                canHover = false;
                currentJumpHoldTime = 0;
                currentCoyoteTime = 0;
            }
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
        return Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask);
    }

    public override IEnumerator Damaged()
    {
        canMove = false;
        canAttack = false;
        canJump = false;

        float damagedTime = 0;
        myBody.velocity = Vector2.zero;
        SetSixFacesFace(sixFaceFaces[2]);
        playerAnimator.ChangeAnimation(bodyAnimator, hurt);

        while(damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if(damagedTime > damagedDuration && damaged)
            {
                damaged = false;
                canMove = true;
                canAttack = true;
                playerAnimator.ChangeAnimation(bodyAnimator, idle);
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
    }

    private void SetSixFacesFace(AnimationClip clip)
    {
        StartCoroutine(SwitchFace(clip));
    }

    IEnumerator SwitchFace(AnimationClip clip)
    {
        if(!faceAnimator.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
        {
            playerAnimator.ChangeAnimation(faceAnimator, changeFace);
        }
        yield return new WaitForSeconds(switchFaceDuration);
        playerAnimator.ChangeAnimation(faceAnimator, clip);
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack)
        {
            playerAnimator.ChangeAnimation(bodyAnimator, upAttack);
            SetSixFacesFace(sixFaceFaces[1]);
            StartCoroutine(SixFaceAttack(upAttackArea));
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.S) && canAttack && (currentJumpHoldTime != 0 || canHover))
        {
            playerAnimator.ChangeAnimation(bodyAnimator, downAttack);
            SetSixFacesFace(sixFaceFaces[5]);
            StartCoroutine(SixFaceAttack(downAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            playerAnimator.ChangeAnimation(bodyAnimator, horizontalAttack);
            SetSixFacesFace(sixFaceFaces[3]);
            StartCoroutine(SixFaceAttack(horizontalAttackArea));
        }
    }

    IEnumerator SixFaceAttack(GameObject attack)
    {
        canAttack = false;
        canMove = false;
        myBody.velocity = new Vector2(0, myBody.velocity.y);
        attack.SetActive(true);
        attack.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attack.GetComponent<Collider2D>().enabled = false;
        attack.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        canMove = true;
        playerAnimator.ChangeAnimation(bodyAnimator, idle);
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
            currentCoyoteTime = coyoteTime;
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);
        }
        else if (isJumping)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * hoverGravityModifier);
            isJumping = false;
            canHover = true;
            SetSixFacesFace(sixFaceFaces[4]);
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

            if (!IsHovering && canAttack && !isJumping)
            {
                playerAnimator.ChangeAnimation(bodyAnimator, move);
                SetSixFacesFace(sixFaceFaces[0]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !horizontalAttackArea.activeInHierarchy)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }

            if(!IsHovering && canAttack && !isJumping)
            {
                playerAnimator.ChangeAnimation(bodyAnimator, move);
                SetSixFacesFace(sixFaceFaces[0]);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
        else
        {
            if (!IsHovering && canAttack && !isJumping && !iFrames)
            {
                playerAnimator.ChangeAnimation(bodyAnimator, idle);
            }
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

            if(canAttack && !downAttackArea.activeInHierarchy)
            {
                playerAnimator.ChangeAnimation(bodyAnimator, hover);
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
