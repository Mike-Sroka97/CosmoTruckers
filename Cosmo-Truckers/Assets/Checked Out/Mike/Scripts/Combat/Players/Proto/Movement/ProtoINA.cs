using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoINA : Player
{
    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject verticalAttackArea;

    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpMaxHoldTime;
    [SerializeField] float coyoteTime;

    [SerializeField] float teleportDistance;
    [SerializeField] float teleportCD;
    [SerializeField] float teleportHoldTime;
    [SerializeField] SpriteRenderer[] teleportSprites;
    [SerializeField] Color teleportSpriteStartingColor;
    [SerializeField] Color invalidteleportColor;
    [SerializeField] float teleportHelperTime;

    [SerializeField] float positiveXBoundary;
    [SerializeField] float positiveYBoundary;
    [SerializeField] float negativeXBoundary;
    [SerializeField] float negativeYBoundary;

    [Space(20)]
    [Header("Animations")]
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip move;
    [SerializeField] AnimationClip jump;
    [SerializeField] AnimationClip attackRight;
    [SerializeField] AnimationClip attackUp;
    [SerializeField] AnimationClip hurt;
    [SerializeField] AnimationClip teleport;
    [SerializeField] GameObject dyingProto;

    [HideInInspector] public bool IsTeleporting { get; private set; }
    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canTeleport = true;

    float currentJumpHoldTime = 0;
    float currentCoyoteTime = 0;
    float currentTPhelperTime = 0;
    int lastTeleportHeld = 0;

    Animator myAnimator;
    PlayerAnimator playerAnimator;
    Collider2D myCollider;
    int layermask = 1 << 9;
    const float distance = 0.05f;

    private void Start()
    {
        PlayerInitialize();
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        playerAnimator = GetComponent<PlayerAnimator>();
        myCollider = GetComponentsInChildren<Collider2D>()[0];
        DebuffInit();
    }

    private void Update()
    {
        UpdateOutline();
        Attack();
        Movement();
        Jump();
        SpecialMove();
    }

    public void SetCanTeleport(bool toggle) { canTeleport = toggle; }

    public void ResetTeleportBoundaries()
    {
        SetTelportBoundaries(positiveXBoundary, positiveYBoundary, negativeXBoundary, negativeYBoundary);
    }

    public override IEnumerator Damaged()
    {
        foreach(SpriteRenderer sprite in teleportSprites)
        {
            sprite.enabled = false;
        }

        myBody.velocity = new Vector2(xVelocityAdjuster, yVelocityAdjuster);
        canJump = false;
        canAttack = false;
        canMove = false;
        canTeleport = false;
        float damagedTime = 0;
        playerAnimator.ChangeAnimation(myAnimator, hurt);

        while (damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                damaged = false;
                canAttack = true;
                canMove = true;
                canTeleport = true;
                playerAnimator.ChangeAnimation(myAnimator, idle);
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
    }

    private void IsGrounded()
    {
        if (Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, distance, layermask))
        {

            if(!damaged && Input.GetKey("space") && canMove)
            {
                canJump = true;
            }
            if(!isJumping)
            {
                currentJumpHoldTime = 0;
                currentCoyoteTime = 0;
            }
        }
        else if(currentCoyoteTime > coyoteTime)
        {
            canJump = false;
        }
        else
        {
            currentCoyoteTime += Time.deltaTime;
        }
    }

    public void SetTelportBoundaries(float newPosX, float newPosY, float newNegX, float newNegY)
    {
        positiveXBoundary = newPosX;
        positiveYBoundary = newPosY;
        negativeXBoundary = newNegX;
        negativeYBoundary = newNegY;
    }

    #region Attack
    /// <summary>
    /// Whacko funny punch go haha
    /// </summary>
    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && canAttack && Input.GetKey(KeyCode.W))
        {
            StartCoroutine(ProtoAttack(false));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(ProtoAttack(true));
        }
    }

    IEnumerator ProtoAttack(bool horizontal)
    {
        canAttack = false;
        canMove = false;
        myBody.velocity = Vector2.zero;

        if(horizontal)
        {
            playerAnimator.ChangeAnimation(myAnimator, attackRight);
            horizontalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            horizontalAttackArea.SetActive(false);
        }
        else
        {
            playerAnimator.ChangeAnimation(myAnimator, attackUp);
            verticalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            verticalAttackArea.SetActive(false);

        }

        playerAnimator.ChangeAnimation(myAnimator, idle);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        canMove = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Proto has normal player jumping
    /// </summary>
    public void Jump()
    {
        IsGrounded();

        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed + yVelocityAdjuster);
            playerAnimator.ChangeAnimation(myAnimator, jump);
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentCoyoteTime = coyoteTime;
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed + yVelocityAdjuster);
        }
        else if (isJumping)
        {
            isJumping = false;
        }
    }
    #endregion

    #region Movement
    /// <summary>
    /// Proto has base player movement characteristics
    /// </summary>
    public void Movement()
    {
        if (!canMove) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) && !horizontalAttackArea.activeInHierarchy)
        {
            myBody.velocity = new Vector2(-moveSpeed + xVelocityAdjuster, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
            if(myBody.velocity.y == 0)
                playerAnimator.ChangeAnimation(myAnimator, move);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !horizontalAttackArea.activeInHierarchy)
        {
            myBody.velocity = new Vector2(moveSpeed + xVelocityAdjuster, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
            if(myBody.velocity.y == 0)
                playerAnimator.ChangeAnimation(myAnimator, move);
        }
        else
        {
            myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
            if(myBody.velocity == Vector2.zero && !IsTeleporting)
                playerAnimator.ChangeAnimation(myAnimator, idle);
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>
    public void SpecialMove()
    {
        if(Input.GetKey(KeyCode.Mouse1) && canTeleport)
        {
            TeleportSprites();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && canTeleport)
        {
            foreach (SpriteRenderer sprite in teleportSprites)
            {
                sprite.enabled = false;
            }
            Teleport();
        }
    }

    private void TeleportSprites()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            if (teleportSprites[0].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[0].enabled = true;
            }
            if ((transform.position.x - teleportDistance < negativeXBoundary) || (transform.position.y + teleportDistance > positiveYBoundary))
            {
                teleportSprites[0].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[0].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 1;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            if (teleportSprites[1].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[1].enabled = true;
            }
            if ((transform.position.x - teleportDistance < negativeXBoundary) || (transform.position.y - teleportDistance < negativeYBoundary))
            {
                teleportSprites[1].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[1].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 2;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            if (teleportSprites[2].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[2].enabled = true;
            }
            if ((transform.position.x + teleportDistance > positiveXBoundary) || (transform.position.y - teleportDistance < negativeYBoundary))
            {
                teleportSprites[2].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[2].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 3;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            if (teleportSprites[3].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[3].enabled = true;
            }
            if ((transform.position.x + teleportDistance > positiveXBoundary) || (transform.position.y + teleportDistance > positiveYBoundary))
            {
                teleportSprites[3].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[3].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 4;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (teleportSprites[4].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[4].enabled = true;
            }
            if ((transform.position.x - teleportDistance < negativeXBoundary))
            {
                teleportSprites[4].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[4].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 5;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (teleportSprites[5].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[5].enabled = true;
            }
            if ((transform.position.y + teleportDistance > positiveYBoundary))
            {
                teleportSprites[5].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[5].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 6;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (teleportSprites[6].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[6].enabled = true; 
            }
            if ((transform.position.y - teleportDistance < negativeYBoundary))
            {
                teleportSprites[6].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[6].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 7;
            currentTPhelperTime = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (teleportSprites[7].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[7].enabled = true;
            }
            if ((transform.position.x + teleportDistance > positiveXBoundary))
            {
                teleportSprites[7].color = invalidteleportColor;
            }
            else
            {
                teleportSprites[7].color = teleportSpriteStartingColor;
            }
            lastTeleportHeld = 8;
            
            currentTPhelperTime = 0;
        }
        else
        {
            currentTPhelperTime += Time.deltaTime;

            if(currentTPhelperTime >= teleportHelperTime)
            {
                ResetTeleportSprites();
            }
        }
    }
    private void ResetTeleportSprites()
    {
        foreach (SpriteRenderer sprite in teleportSprites)
        {
            lastTeleportHeld = 0;
            sprite.color = teleportSpriteStartingColor;
            sprite.enabled = false;
        }
    }

    private void Teleport()
    {
        if((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)) || lastTeleportHeld == 1)
        {
            if(!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)) || lastTeleportHeld == 2)
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)) || lastTeleportHeld == 3)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)) || lastTeleportHeld == 4)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.A) || lastTeleportHeld == 5)
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.W) || lastTeleportHeld == 6)
        {
            if (!(transform.position.y + teleportDistance > positiveYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(0, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.S) || lastTeleportHeld == 7)
        {
            if (!(transform.position.y - teleportDistance < negativeYBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(0, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.D) || lastTeleportHeld == 8)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }

        lastTeleportHeld = 0;
        currentTPhelperTime = 0;
    }

    private void CreateDyingProto()
    {
        GameObject remnant = Instantiate(dyingProto, transform);
        remnant.transform.parent = null;
    }

    IEnumerator TeleportCooldown()
    {
        ResetTeleportSprites();
        myBody.velocity = Vector2.zero;
        currentJumpHoldTime = jumpMaxHoldTime;
        canTeleport = false;
        myBody.gravityScale = 0;
        canMove = false;
        IsTeleporting = true;
        playerAnimator.ChangeAnimation(myAnimator, teleport);

        yield return new WaitForSeconds(teleportHoldTime);

        IsTeleporting = false;
        if(!damaged)
        {
            canMove = true;
        }
        myBody.gravityScale = initialGravityModifier;

        yield return new WaitForSeconds(teleportCD - teleportHoldTime);

        canTeleport = true;
    }

    #endregion
}
