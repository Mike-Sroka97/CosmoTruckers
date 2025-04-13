using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarINA : Player
{
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCoolDown;
    [SerializeField] float dashUpForce;

    //mech helpers
    public bool DashingLeft { get; private set; }
    public bool DashingRight { get; private set; }
    public bool DashingUp { get; private set; }

    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject verticalAttackArea;
    [SerializeField] int numberOfAttacks = 2;
    [SerializeField] int numberOfJumps = 2;

    [Space(20)]
    [Header("Animations")]
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip move;
    [SerializeField] AnimationClip dashRight;
    [SerializeField] AnimationClip dashUp;
    [SerializeField] AnimationClip hurt;

    bool canDash = true;
    bool canMove = true;

    Collider2D myCollider;
    Animator myAnimator;
    PlayerAnimator playerAnimator;
    int layermask = 1 << 9; //ground
    const float distance = 0.05f;
    int currentNumberOfAttacks = 0;
    int currentNumberOfJumps = 0;
    InputManager inputManager; 

    private void Start()
    {
        PlayerInitialize();

        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        myCollider = transform.Find("AeglarBody").GetComponent<Collider2D>();
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        UpdateOutline();
        Attack();
        Movement();
        Jump();
    }

    private bool IsGrounded(float distance)
    {
        Vector2 size = new Vector2(myCollider.bounds.size.x / 2, myCollider.bounds.size.y);

        if (Physics2D.BoxCast(myCollider.bounds.center, size, 0, Vector2.down, distance, layermask))
            return true;

        else return false;
    }

    public bool GetDashState() { return canDash; }

    public override IEnumerator Damaged()
    {
        float damagedTime = 0;
        canDash = false;
        canMove = false;
        playerAnimator.ChangeAnimation(myAnimator, hurt);

        while (damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime;
            if (damagedTime > damagedDuration && !dead && damaged)
            {
                canDash = true;
                canMove = true;
                damaged = false;
                playerAnimator.ChangeAnimation(myAnimator, idle);
            }
            yield return null;
        }

        if(!dead)
            iFrames = false;
    }

    public override void EndMoveSetup()
    {
        playerAnimator.ChangeAnimation(myAnimator, idle);
        base.EndMoveSetup();
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if (currentNumberOfAttacks < numberOfAttacks)
        {
            if (inputManager.AttackPressed && canDash && transform.localEulerAngles.y != 0 && !DashingLeft && !DashingRight)
            {
                CleanUpDash();
                StartCoroutine(Dash(true));
            }
            else if (inputManager.AttackPressed && canDash && transform.localEulerAngles.y == 0 && !DashingLeft && !DashingRight)
            {
                CleanUpDash();
                StartCoroutine(Dash(false));
            }
        }

    }
    #endregion

    #region Jump
    /// <summary>
    /// Aeglar will not be able to move while jumping. He can still dash
    /// </summary>
    public void Jump()
    {
        if (damaged || dead)
            return;

        if (IsGrounded(0.05f) && canDash)
        {
            currentNumberOfAttacks = 0;
            currentNumberOfJumps = 0;
        }

        if (inputManager.JumpPressed && currentNumberOfJumps < numberOfJumps && !DashingUp)
        {
            CleanUpDash();
            StartCoroutine(Dash(true, true));
        }
    }
    #endregion

    #region Movement
    /// <summary>
    /// Aeglar's movement will cause short spurts of movements. There is an associated cooldown to this
    /// </summary>
    public void Movement()
    {
        if (!canMove || damaged || dead) return;

        if (inputManager.MoveInput.x < 0)
        {
            myBody.velocity = new Vector2(-MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
            playerAnimator.ChangeAnimation(myAnimator, move);

            if (transform.rotation.eulerAngles.y == 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (inputManager.MoveInput.x > 0)
        {
            myBody.velocity = new Vector2(MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
            playerAnimator.ChangeAnimation(myAnimator, move);

            if (transform.rotation.eulerAngles.y != 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
        }
        else
        {
            myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
            playerAnimator.ChangeAnimation(myAnimator, idle);
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>

    IEnumerator Dash(bool left, bool up = false)
    {
        myAudioDevice.PlaySound("Dash"); 

        if (up)
        {
            currentNumberOfJumps++;
            verticalAttackArea.SetActive(true);
            playerAnimator.ChangeAnimation(myAnimator, dashUp);
            DashingUp = true;
        }
        else
        {
            currentNumberOfAttacks++;
            horizontalAttackArea.SetActive(true);
            playerAnimator.ChangeAnimation(myAnimator, dashRight);
            if (left)
                DashingLeft = true;
            else
                DashingRight = true;
        }

        canMove = false;

        myBody.velocity = new Vector2(xVelocityAdjuster, yVelocityAdjuster);

        if (up)
            myBody.AddForce(new Vector2(0, jumpSpeed));
        else if (left)
            myBody.AddForce(new Vector2(-dashSpeed, dashUpForce));
        else
            myBody.AddForce(new Vector2(dashSpeed, dashUpForce));

        float currentDashTime = 0;
        while (currentDashTime < dashDuration)
        {
            if (up)
            {
                if (currentDashTime > dashCoolDown && currentNumberOfJumps < numberOfJumps)
                    canDash = true;
                myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
            }
            else
            {
                if (currentDashTime > dashCoolDown && currentNumberOfAttacks < numberOfAttacks)
                    canDash = true;
                myBody.velocity = new Vector2(myBody.velocity.x, yVelocityAdjuster);
            }

            currentDashTime += Time.deltaTime;
            yield return null;
        }

        CleanUpDash();
    }
    private void CleanUpDash()
    {
        canMove = true;
        myBody.velocity = new Vector2(xVelocityAdjuster, yVelocityAdjuster);
        verticalAttackArea.SetActive(false);
        horizontalAttackArea.SetActive(false);
        DashingUp = false;
        DashingLeft = false;
        DashingRight = false;

        StopAllCoroutines();
    }

    public void SpecialMove()
    {

    }
    #endregion
}
