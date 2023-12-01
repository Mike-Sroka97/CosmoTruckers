using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] BoxCollider2D headBody;
    [SerializeField] BoxCollider2D bodyBody;

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

    LongDogNeck currentLine;

    bool stretching = false;
    bool buttStretching = false;
    bool canStretch = true; //make sure to set this to false ONLY when ass is retracting to skull
    bool canMove = true;
    bool goingLeft = false;
    bool goingRight = false;
    bool startupStretch = false;
    bool canBark = true;

    Vector3 buttStartingLocation;
    float startingGravityScale;
    int layermask = 1 << 9;
    Collider2D myCollider;
    PlayerAnimator playerAnimator;

    private void Start()
    {
        PlayerInitialize();

        myBody = head.GetComponent<Rigidbody2D>();
        myCollider = head.GetComponent<Collider2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        buttStartingLocation = body.transform.localPosition;
        startingGravityScale = myBody.gravityScale;
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
        EndDraw();
        damaged = toggle;
    }

    public void StretchingCollision(string collision)
    {
        if(collision != "LDGNoInteraction")
        {
            if (stretching)
            {
                stretching = false;
                myNose.enabled = false;
                EndDraw();
            }
        }
    }

    public override IEnumerator Damaged()
    {
        canMove = false;
        canStretch = false;
        iFrames = true;
        float damagedTime = 0;
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
            if(damagedTime >= damagedDuration)
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && canStretch)
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
        else if(Input.GetKey(KeyCode.Mouse0) && stretching)
        {
            if(currentLine != null)
            {
                Draw();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0) && stretching)
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

    void BeginDraw()
    {
        attackArea.SetActive(true);
        stretching = true;
        myNose.enabled = true;
        body.transform.SetParent(transform);
        myBody.gravityScale = 0;

        currentLine = Instantiate(linePrefab, transform).GetComponent<LongDogNeck>();
    }

    void Draw()
    {
        currentLine.AddPoint(head.transform.localPosition);
    }
    void EndDraw()
    {
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
        myBody.gravityScale = startingGravityScale;

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
        yield return new WaitForSeconds(postSpinCD);

        body.transform.localPosition = buttStartingLocation;
        head.transform.localRotation = new Quaternion(0, head.transform.localRotation.y, 0, 0);
        body.transform.localRotation = new Quaternion(0, 0, 0, 0);
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
    }
    #endregion

    #region Jump
    /// <summary>
    /// Long Dog is a stupid dog and does not how to jump with his pathetic legs
    /// </summary>
    public void Jump()
    {

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
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
                {
                    goingLeft = false;
                    goingRight = true;
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
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
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
                {
                    goingLeft = true;
                    goingRight = false;
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
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

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
            {
                myBody.velocity = new Vector2(-MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
                if (head.transform.eulerAngles != new Vector3(0, 0, 0))
                {
                    head.transform.eulerAngles = new Vector3(0, 0, 0);
                    head.transform.position += new Vector3(positionFlipModifier, 0, 0);
                }
                playerAnimator.ChangeAnimation(bodyAnimator, moveBody);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
            {
                myBody.velocity = new Vector2(MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
                if(head.transform.eulerAngles != new Vector3(0, 180, 0))
                {
                    head.transform.eulerAngles = new Vector3(0, 180, 0);
                    head.transform.position -= new Vector3(positionFlipModifier, 0, 0);
                }
                playerAnimator.ChangeAnimation(bodyAnimator, moveBody);
            }
            else if(IsGrounded())
            {
                myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
                playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
            }
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(head.transform.position, Vector2.down, myCollider.bounds.extents.y + .25f, layermask))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Bark for invincibility
    /// </summary>
    public void SpecialMove()
    {
        if(canBark && !stretching && !damaged && Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(Bark());
        }
    }

    IEnumerator Bark()
    {
        playerAnimator.ChangeAnimation(headAnimator, barkHead);
        playerAnimator.ChangeAnimation(bodyAnimator, idleBody);
        canBark = false;
        canMove = false;
        canStretch = false;
        iFrames = true;
        myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);

        yield return new WaitForSeconds(barkDuration);

        canMove = true;
        canStretch = true;
        iFrames = false;
        playerAnimator.ChangeAnimation(headAnimator, idleHead);

        yield return new WaitForSeconds(barkCooldown);

        canBark = true;
    }
    #endregion
}
