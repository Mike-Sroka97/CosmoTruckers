using System.Collections;
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
    [SerializeField] LineRenderer myLineRenderer;
    [SerializeField] float rollCD;
    [SerializeField] float rollDuration;
    [SerializeField] float rollSpeed;

    [SerializeField] float hopForceModifier;
    [SerializeField] float raycastHopHelper;
    
    /// <summary>
    /// This is for shorthopping when you barely press the jump hold<br></br>
    /// The minimum value a hop can be
    /// </summary>
    [SerializeField] float hopMinimumStrength;

    [Space(20)]
    [Header("Animations")]
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip move;
    [SerializeField] AnimationClip punchRight;
    [SerializeField] AnimationClip punchUp;
    [SerializeField] AnimationClip coil;
    [SerializeField] AnimationClip jump;
    [SerializeField] AnimationClip hurt;
    [SerializeField] AnimationClip roll;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canRoll = true;
    bool damagedCoroutineRunning = false;  

    float currentJumpStrength;
    float currentJumpHoldTime = 0;
    const float startingHeight = .1f;

    int layermask = 1 << 9; //ground

    Collider2D myCollider;
    Animator myAnimator;
    PlayerAnimator playerAnimator;

    float originalMoveSpeed;

    private void Start()
    {
        PlayerInitialize();

        myAnimator = GetComponentInChildren<Animator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        originalMoveSpeed = MoveSpeed;
        currentJumpStrength = 0;
        myBody = GetComponent<Rigidbody2D>();
        myCollider = transform.Find("Body").GetComponent<Collider2D>();
    }

    private void Update()
    {
        UpdateOutline();
        Attack();
        Movement();
        Jump();
        SpecialMove();
    }

    public void SetMoveSpeed(float newSpeed) { MoveSpeed = newSpeed; }
    public float GetMoveSpeed() { return MoveSpeed; }
    public bool GetIsJumping() { return isJumping; }

    public override IEnumerator Damaged()
    {
        damagedCoroutineRunning = true; 

        playerAnimator.ChangeAnimation(myAnimator, hurt);
        HandleLineRenderer(startingHeight);
        canAttack = false;
        canMove = false;
        canJump = false;
        float damagedTime = 0;

        while (damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration && damaged && !dead)
            {
                damaged = false;
                playerAnimator.ChangeAnimation(myAnimator, idle);
                canAttack = true;
                canMove = true;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        if (!dead)
            iFrames = false;

        damagedCoroutineRunning = false; 
    }

    public override void EndMoveSetup()
    {
        playerAnimator.ChangeAnimation(myAnimator, idle);
        myLineRenderer.enabled = false; 
        base.EndMoveSetup();
    }

    #region Attack
    /// <summary>
    /// Regular horizontal and vertical attacks
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack && !isJumping)
        {
            playerAnimator.ChangeAnimation(myAnimator, punchUp);
            StartCoroutine(SafeTAttack(upAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && !isJumping)
        {
            playerAnimator.ChangeAnimation(myAnimator, punchRight, speed: 2);
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
        if (IsGrounded(raycastHopHelper) && !damaged && !dead)
            canJump = true;

        // Hold Jump
        if (Input.GetKey("space") && canJump && !isJumping)
        {
            canMove = false;
            canJump = false;
            isJumping = true;
            playerAnimator.ChangeAnimation(myAnimator, coil);
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            if (!playerAnimator.IsCurrentAnimationPlaying(myAnimator, coil))
                playerAnimator.ChangeAnimation(myAnimator, coil);

            currentJumpHoldTime += Time.deltaTime;
            iFrames = true;
            if(currentJumpHoldTime > jumpMaxHoldTime) 
            {
                iFrames = false;
                currentJumpHoldTime = jumpMaxHoldTime; 
            }
            if(IsGrounded(raycastHopHelper))
            {
                HandleLineRenderer(currentJumpHoldTime);
            }
            else
            {
                HandleLineRenderer(startingHeight);
            }
            currentJumpStrength = currentJumpHoldTime * jumpSpeedAccrual;
            myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
        }
        
        // Release Jump
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space") && (IsGrounded(raycastHopHelper)))
        {
            playerAnimator.ChangeAnimation(myAnimator, jump);
            HandleLineRenderer(startingHeight);

            if (currentJumpStrength > jumpSpeed)
                currentJumpStrength = jumpSpeed;

            float jumpStrength = currentJumpStrength; 
            if (jumpStrength < hopMinimumStrength) jumpStrength = hopMinimumStrength;

            myBody.velocity = new Vector2(myBody.velocity.x, yVelocityAdjuster);
            myBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            isJumping = false;

            if (!damagedCoroutineRunning)
            {
                iFrames = false;
            }
        }
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space"))
        {
            HandleLineRenderer(startingHeight);
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            isJumping = false;

            if (!damagedCoroutineRunning)
            {
                iFrames = false;
            }
        }
    }

    private void HandleLineRenderer(float pointTwoPosition)
    {
        if(pointTwoPosition == startingHeight)
        {
            myLineRenderer.SetPosition(1, new Vector3(0, startingHeight, 0));
        }
        else
        {
            //Don't ask
            myLineRenderer.SetPosition(1, new Vector3(0, pointTwoPosition * currentJumpHoldTime * 2, 0));
        }
    }

    private bool IsGrounded(float distance)
    {
        if (Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, distance, layermask))
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
        if (!canMove || damaged || dead) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            myBody.velocity = new Vector2(-MoveSpeed + xVelocityAdjuster, myBody.velocity.y);

            if (transform.rotation.eulerAngles.y == 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
            if (myBody.velocity.y <= 0 && canAttack)
            {
                playerAnimator.ChangeAnimation(myAnimator, move);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            myBody.velocity = new Vector2(MoveSpeed + xVelocityAdjuster, myBody.velocity.y);

            if (transform.rotation.eulerAngles.y != 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
            if (myBody.velocity.y <= 0 && canAttack)
            {
                playerAnimator.ChangeAnimation(myAnimator, move);
            }
        }
        else if(!isJumping)
        {
            if(canAttack && canJump && canMove && myBody.velocity.y <= 0)
            {
                playerAnimator.ChangeAnimation(myAnimator, idle);
            }
            myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
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
            HandleLineRenderer(startingHeight);
            isJumping = false;
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;

            if (!damagedCoroutineRunning)
            {
                iFrames = false;
            }
        }
        else if(!isJumping && Input.GetKeyDown(KeyCode.Mouse1) && canRoll && (myBody.velocity.x > 0 || myBody.velocity.x < 0))
        {
            StartCoroutine(Roll());
            canMove = false;
        }
    }

    IEnumerator Roll()
    {
        if(myBody.velocity.x > 0)
        {
            myBody.velocity = new Vector2(rollSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(-rollSpeed, myBody.velocity.y);
        }

        playerAnimator.ChangeAnimation(myAnimator, roll);

        canJump = false;
        canRoll = false;
        canMove = false;
        iFrames = true;

        yield return new WaitForSeconds(rollDuration);

        canMove = true;
        if (!damagedCoroutineRunning)
        {
            iFrames = false;
        }
        playerAnimator.ChangeAnimation(myAnimator, idle);

        yield return new WaitForSeconds(rollCD);

        canRoll = true;
    }
    #endregion
}
