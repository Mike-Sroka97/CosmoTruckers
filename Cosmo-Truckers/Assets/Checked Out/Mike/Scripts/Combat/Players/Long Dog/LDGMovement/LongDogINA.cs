using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.Image;

public class LongDogINA : Player
{
    [SerializeField] float stretchSpeed;
    [SerializeField] float stretchReturnSpeed;
    [SerializeField] float stretchRotateSpeed;
    [SerializeField] float positionFlipModifier;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    [SerializeField] GameObject linePrefab;
    [SerializeField] Gradient lineColor;
    [SerializeField] float linePointsMinDistance;
    [SerializeField] float lineWidth;
    [SerializeField] float spinForceBoost;
    [SerializeField] float spinSpeed;
    [SerializeField] float postSpinCD;
    [SerializeField] float stretchStartupTime;
    [SerializeField] Collider2D myNose;
    [SerializeField] float barkCooldown;
    [SerializeField] float barkDuration;
    [SerializeField] GameObject attackArea;
    [SerializeField] GameObject barkArea;
    [SerializeField] BoxCollider2D headBody;
    [SerializeField] BoxCollider2D bodyBody;
    [SerializeField] Transform neckSpawn;

    [Space(20)]
    [Header("Animation")]
    [SerializeField] Animator headAnimator;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] AnimationClip idleHead;
    [SerializeField] AnimationClip hurtHead;
    [SerializeField] AnimationClip stretchingHead;
    [SerializeField] AnimationClip barkHead;
    [SerializeField] AnimationClip idleBody;
    [SerializeField] AnimationClip hurtBody;
    [SerializeField] AnimationClip moveBody;
    [SerializeField] AnimationClip stretchBody;

    [Space(20)]
    [Header("Jump")]
    [SerializeField] float jumpMaxHoldTime;
    [SerializeField] float coyoteTime;

    LongDogNeck currentLine;
        [HideInInspector]
    public UnityEvent FlipEvent = new UnityEvent();
    [SerializeField] private float maxStretchPitch = 3f; 
    [SerializeField] private float stretchPitchRate = 3f; 

    bool canJump = false;
    bool isJumping = false;
    bool stretching = false; 
    bool buttStretching = false;
    bool canStretch = true; //make sure to set this to false ONLY when ass is retracting to skull
    bool canMove = true;
    bool goingLeft = false;
    bool goingRight = false;
    bool startupStretch = false;
    bool canBark = true;
    bool postSpin = false;

    Vector3 buttStartingLocation;
    float currentJumpHoldTime = 0;
    float currentCoyoteTime = 0;
    float damagedTime;
    int layermask = 1 << 9;
    Collider2D myCollider;
    PlayerAnimator playerAnimator;

    private void Start()
    {
        myBody = head.GetComponent<Rigidbody2D>();
        myCollider = head.GetComponent<Collider2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        buttStartingLocation = body.transform.localPosition;
        initialGravityModifier = myBody.gravityScale;
        damagedTime = iFrameDuration;

        PlayerInitialize();
    }

    private void Update()
    {
        UpdateOutline();
        Attack();
        Movement();
        Jump();
        SpecialMove();
    }

    public void SetDamaged(bool toggle)
    {
        stretching = false;
        myNose.enabled = false;
        myNose.GetComponent<LongDogNose>().enabled = false;
        EndDraw();
        damaged = toggle;
    }

    public GameObject GetHead() { return head; }

    public void StretchingCollision(string collision)
    {
        if(collision != "LDGNoInteraction")
            ButtStuff();
    }

    private void ButtStuff()
    {
        if (stretching)
        {
            stretching = false;
            myNose.enabled = false;
            myNose.GetComponent<LongDogNose>().enabled = false;
            EndDraw();
        }
    }

    public override IEnumerator Damaged()
    {
        ButtStuff();

        canMove = false;
        canStretch = false;
        iFrames = true;
        damagedTime = 0;
        playerAnimator.ChangeAnimation(headAnimator, hurtHead);
        playerAnimator.ChangeAnimation(bodyAnimator, hurtBody);

        while(buttStretching)
        {
            yield return null;
        }

        myBody.velocity = new Vector2(xVelocityAdjuster, yVelocityAdjuster);

        while (damagedTime < damagedDuration)
        {
            yield return null;

            damagedTime += Time.deltaTime;
            if(damagedTime >= damagedDuration && !dead)
            {
                playerAnimator.ChangeAnimation(headAnimator, idleHead);
                playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
                damaged = false;
                LDGReset();
            }
        }
        while(damagedTime < iFrameDuration)
        {
            yield return null;

            damagedTime += Time.deltaTime;
        }

        if(!dead)
            iFrames = false;
    }

    public void SetCanMove(bool toggle) { canMove = toggle; }
    public bool GetStretching() { return stretching; }

    #region Attack
    /// <summary>
    /// Long Dogs Attack will be his head being an active hitbox while he stetches. This attack function will handle the stretching
    /// </summary>
    public void Attack()
    {
        if (inputManager.AttackPressed && canStretch && !IsGrounded() && !iFrames)
        {
            canStretch = false;
            myBody.velocity = Vector2.zero;
            playerAnimator.ChangeAnimation(headAnimator, stretchingHead);
            playerAnimator.ChangeAnimation(bodyAnimator, stretchBody);
            headBody.enabled = false;
            bodyBody.enabled = true;
            StartCoroutine(StartStretch());
            BeginDraw();
        }
        else if(inputManager.AttackHeld && stretching)
        {
            if(currentLine != null)
            {
                Draw();
            }
        }
        else if(inputManager.AttackAction.WasReleasedThisFrame() && stretching)
        {
            EndDraw();
        }
    }

    IEnumerator StartStretch()
    {
        startupStretch = true;
        yield return new WaitForSeconds(stretchStartupTime);
        startupStretch = false;
    }
    
    private IEnumerator StretchingSound()
    {
        AudioSource stretch = myAudioDevice.PlaySound("Stretch");
        float originalPitch = 1f;
        float timer = 0f; 

        while (stretching && stretch.pitch < maxStretchPitch)
        {
            timer += Time.deltaTime;
            stretch.pitch = Mathf.Lerp(originalPitch, maxStretchPitch, timer / stretchPitchRate);
            yield return null; 
        }
    }

    private void ResetStretchingSound()
    {
        AudioSource stretch = myAudioDevice.StopSound("Stretch");
        stretch.pitch = 1f; 
    }

    void BeginDraw()
    {
        attackArea.SetActive(true);
        stretching = true;
        myNose.enabled = true; myNose.GetComponent<LongDogNose>().enabled = true;
        body.transform.SetParent(transform);
        myBody.gravityScale = 0;
        StartCoroutine(StretchingSound());

        currentLine = Instantiate(linePrefab, neckSpawn.localPosition, neckSpawn.localRotation, transform).GetComponent<LongDogNeck>();
    }

    void Draw()
    {
        currentLine.AddPoint(head.transform.position);
    }
    void EndDraw()
    {
        ResetStretchingSound();
        attackArea.SetActive(false);
        headBody.enabled = true;
        bodyBody.enabled = false;
        buttStretching = true;
        body.GetComponent<LongDogButt>().StartButtToHeadMovement();
    }

    public void ATHDone()
    {
        body.transform.parent = head.transform;
        body.transform.localPosition = buttStartingLocation;
        myBody.gravityScale = initialGravityModifier;

        LongDogNeck[] longDogNecks = FindObjectsOfType<LongDogNeck>();
        foreach(LongDogNeck neck in longDogNecks)
        {
            Destroy(neck.gameObject);
        }

        StartCoroutine(ATHSpin());
    }

    public void SetupLineRenderer(LongDogNeck mCurrentLine)
    {
        mCurrentLine.SetLineColor(lineColor);
        mCurrentLine.SetPointsMinDistance(linePointsMinDistance);
        mCurrentLine.SetLineWidth(lineWidth);
    }

    IEnumerator ATHSpin()
    {
        postSpin = true;
        iFrames = true;
        bool completedRotation = false;
        float currentDegrees = 0;
        bool leftBoost;

        if (head.transform.rotation.y == 0)
        {
            leftBoost = (head.transform.localRotation.eulerAngles.z < 90 || head.transform.localRotation.eulerAngles.z > 270);
            if (!leftBoost)
            {
                head.transform.eulerAngles = new Vector3(0, -180, 0);
            }
        }
        else
        {
            leftBoost = (head.transform.localRotation.eulerAngles.z > 90 && head.transform.localRotation.eulerAngles.z < 270);
            if (leftBoost)
            {
                head.transform.eulerAngles = Vector3.zero;
            }
        }

        head.transform.localRotation = new Quaternion(0, head.transform.localRotation.y, 0, 0);
        body.transform.localRotation = new Quaternion(0, 0, 0, 0);

        if (leftBoost)
        {
            myBody.AddForce(new Vector2(-spinForceBoost, spinForceBoost), ForceMode2D.Impulse);
        }
        else
        {
            myBody.AddForce(new Vector2(spinForceBoost, spinForceBoost), ForceMode2D.Impulse);
        }
        while (!completedRotation)
        {
            head.transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
            yield return new WaitForSeconds(Time.deltaTime);
            currentDegrees += spinSpeed * Time.deltaTime;
            if (currentDegrees >= 360)
            {
                completedRotation = true;
            }
        }

        FlipEvent?.Invoke(); 
        yield return new WaitForSeconds(postSpinCD);

        body.transform.localPosition = buttStartingLocation;
        head.transform.localRotation = new Quaternion(0, head.transform.localRotation.y, 0, 0);
        body.transform.localRotation = new Quaternion(0, 0, 0, 0);

        if(damagedTime >= iFrameDuration)
            iFrames = false;

        LDGReset();
    }

    public void LDGReset()
    {
        canBark = true;
        canMove = true;
        canStretch = true;
        buttStretching = false;
        stretching = false;
        myNose.enabled = false;
        myNose.GetComponent<LongDogNose>().enabled = false;
        ResetStretchingSound(); 
    }

    public void LDGSpecialDeath()
    {
        headBody.enabled = false;
        bodyBody.enabled = false;
        canBark = false;
        canMove = false;
        canStretch = false;
        buttStretching = false;
        stretching = false;
        myNose.enabled = false;
        myNose.GetComponent<LongDogNose>().enabled = false;
        ResetStretchingSound(); 
    }

    public override void EndMoveSetup()
    {
        EndDraw();
        base.EndMoveSetup();
    }

    #endregion

    #region Jump
    /// <summary>
    /// Long Dog is a stupid dog and does not how to jump with his pathetic legs
    /// </summary>
    public void Jump()
    {
        if (stretching)
            return;

        IsGroundedJump();

        if (inputManager.JumpPressed && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed + yVelocityAdjuster);
            playerAnimator.ChangeAnimation(bodyAnimator, stretchBody);
        }
        else if (inputManager.JumpHeld && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentCoyoteTime = coyoteTime;
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed + yVelocityAdjuster);
            playerAnimator.ChangeAnimation(bodyAnimator, stretchBody);
        }
        else if (isJumping)
        {
            isJumping = false;
            playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
        }
    }
    #endregion

    #region Movement
    /// <summary>
    /// Long dogs movement is split into normal movement and stretch movement
    /// </summary>
    public void Movement()
    {
        if (!canMove) return;

        if(stretching && !buttStretching && !startupStretch)
        {
            head.transform.Translate(Vector3.left * stretchSpeed * Time.deltaTime);

            if(head.transform.eulerAngles.y != 0)
            {
                if (inputManager.MoveInput.x < 0)
                {
                    goingLeft = false;
                    goingRight = true;
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
                else if (inputManager.MoveInput.x > 0)
                {
                    goingLeft = true;
                    goingRight = false;
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if(goingLeft)
                {
                    goingRight = false;
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if(goingRight)
                {
                    goingLeft = false;
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
            }
            else
            {
                if (inputManager.MoveInput.x < 0)
                {
                    goingLeft = true;
                    goingRight = false;
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if (inputManager.MoveInput.x > 0)
                {
                    goingLeft = false;
                    goingRight = true;
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
                else if (goingLeft)
                {
                    goingRight = false;
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if (goingRight)
                {
                    goingLeft = false;
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
            }
        }
        else if(stretching && startupStretch)
        {
            head.transform.Translate(Vector3.left * stretchSpeed * Time.deltaTime);

            goingLeft = false;
            goingRight = true;
            head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
        }
        else if(!stretching && !buttStretching)
        {
            playerAnimator.ChangeAnimation(headAnimator, idleHead);
            goingLeft = false;
            goingRight = false;

            if (inputManager.MoveInput.x < 0)
            {
                myBody.velocity = new Vector2(-MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
                if (head.transform.eulerAngles != new Vector3(0, 0, 0))
                {
                    head.transform.eulerAngles = new Vector3(0, 0, 0);
                    head.transform.position += new Vector3(positionFlipModifier, 0, 0);
                }

                if (IsGrounded())
                    playerAnimator.ChangeAnimation(bodyAnimator, moveBody);

                if (postSpin)
                    postSpin = false;
            }
            else if (inputManager.MoveInput.x > 0)
            {
                myBody.velocity = new Vector2(MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
                if(head.transform.eulerAngles != new Vector3(0, 180, 0))
                {
                    head.transform.eulerAngles = new Vector3(0, 180, 0);
                    head.transform.position -= new Vector3(positionFlipModifier, 0, 0);
                }

                if (IsGrounded())
                    playerAnimator.ChangeAnimation(bodyAnimator, moveBody);

                if (postSpin)
                    postSpin = false;
            }
            else if(!postSpin || IsGrounded())
            {
                postSpin = false;
                myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
                if(IsGrounded())
                    playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
            }
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, .05f, layermask))
        {
            return true;
        }
        return false;
    }

    private void IsGroundedJump()
    {
        if (Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, .05f, layermask))
        {

            if (!damaged && inputManager.JumpHeld && canMove && !stretching && !dead)
            {
                canJump = true;
            }
            if (!isJumping)
            {
                currentJumpHoldTime = 0;
                currentCoyoteTime = 0;
            }
        }
        else if (currentCoyoteTime > coyoteTime)
        {
            canJump = false;
        }
        else
        {
            currentCoyoteTime += Time.deltaTime;
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Bark for invincibility
    /// </summary>
    public void SpecialMove()
    {
        if(canBark && !stretching && !damaged && !dead && inputManager.SpecialPressed)
        {
            StartCoroutine(Bark());
        }
    }

    IEnumerator Bark()
    {
        MyRenderers[0].enabled = false; // Disable head renderer
        MyRenderers[2].enabled = true; // Enable bark renderer

        barkArea.SetActive(true);
        playerAnimator.ChangeAnimation(headAnimator, barkHead);
        playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
        canBark = false;
        canMove = false;
        canStretch = false;
        iFrames = true;
        myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);

        yield return new WaitForSeconds(barkDuration);

        MyRenderers[0].enabled = true; // Enable head renderer
        MyRenderers[2].enabled = false; // Disable bark renderer

        barkArea.SetActive(false);
        canMove = true;
        canStretch = true;
        iFrames = false;
        playerAnimator.ChangeAnimation(headAnimator, idleHead);

        yield return new WaitForSeconds(barkCooldown);

        canBark = true;
    }
    #endregion
}
