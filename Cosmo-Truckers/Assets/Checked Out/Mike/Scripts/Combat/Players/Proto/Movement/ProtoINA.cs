using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoINA : Player
{
    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject verticalAttackArea;

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
    [SerializeField] AnimationClip unchargedIdle;
    [SerializeField] AnimationClip move;
    [SerializeField] AnimationClip unchargedMove;
    [SerializeField] AnimationClip jump;
    [SerializeField] AnimationClip unchargedJump;
    [SerializeField] AnimationClip attackRight;
    [SerializeField] AnimationClip unchargedAttackRight;
    [SerializeField] AnimationClip attackUp;
    [SerializeField] AnimationClip unchargedAttackUp;
    [SerializeField] AnimationClip hurt;
    [SerializeField] AnimationClip unchargedHurt;
    [SerializeField] AnimationClip teleport;
    [SerializeField] GameObject dyingProto;

    [HideInInspector] public bool IsTeleporting { get; private set; }
    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool isTeleporting = false;
    bool canAttack = true;
    bool canTeleport = true;
    bool isGrounded = false; 

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

        // Set Proto's boundaries using the new INA position
        CombatMove minigame = FindObjectOfType<CombatMove>();
        SetTelportBoundaries(positiveXBoundary + minigame.transform.position.x, positiveYBoundary + minigame.transform.position.y, negativeXBoundary + minigame.transform.position.x, negativeYBoundary + minigame.transform.position.y); 
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
        if (canTeleport)
            playerAnimator.ChangeAnimation(myAnimator, hurt);
        else
            playerAnimator.ChangeAnimation(myAnimator, unchargedHurt);

        while (damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration && damaged && !dead)
            {
                damaged = false;
                canAttack = true;
                canMove = true;
                canTeleport = true;

                if(canTeleport)
                    playerAnimator.ChangeAnimation(myAnimator, idle);
                else
                    playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        if (!dead)
            iFrames = false;
    }

    private void IsGrounded()
    {
        if (Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, distance, layermask))
        {
            if (!damaged && !dead && inputManager.JumpPressed && canMove)
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(jump.name) && currentJumpHoldTime >= jumpMaxHoldTime)
                    if (canTeleport)
                        playerAnimator.ChangeAnimation(myAnimator, idle);
                    else
                        playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);

                canJump = true;
            }
            if(!isJumping)
            {
                currentJumpHoldTime = 0;
                currentCoyoteTime = 0;
            }

            isGrounded = true; 
        }
        else if(currentCoyoteTime > coyoteTime)
        {
            canJump = false;
            isGrounded = false;
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

    public override void EndMoveSetup()
    {
        if (canTeleport)
            playerAnimator.ChangeAnimation(myAnimator, idle);
        else
            playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);
        base.EndMoveSetup();
    }

    #region Attack
    /// <summary>
    /// Whacko funny punch go haha
    /// </summary>
    public void Attack()
    {
        if (isTeleporting)
            return;

        // Attack up
        if(inputManager.AttackPressed && canAttack && inputManager.MoveInput.y > 0)
        {
            StartCoroutine(ProtoAttack(false));
        }
        // Regular attack
        else if (inputManager.AttackPressed && canAttack)
        {
            StartCoroutine(ProtoAttack(true));
        }
    }

    IEnumerator ProtoAttack(bool horizontal)
    {
        canAttack = false;

        if(horizontal)
        {
            if (canTeleport)
                playerAnimator.ChangeAnimation(myAnimator, attackRight);
            else
                playerAnimator.ChangeAnimation(myAnimator, unchargedAttackRight);
            horizontalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            horizontalAttackArea.SetActive(false);
        }
        else
        {
            if (canTeleport)
                playerAnimator.ChangeAnimation(myAnimator, attackUp);
            else
                playerAnimator.ChangeAnimation(myAnimator, unchargedAttackUp);
            verticalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            verticalAttackArea.SetActive(false);

        }

        if (canTeleport)
            playerAnimator.ChangeAnimation(myAnimator, idle);
        else
            playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Proto has normal player jumping
    /// </summary>
    public void Jump()
    {
        if (isTeleporting)
            return;

        IsGrounded();

        if (inputManager.JumpPressed && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed + yVelocityAdjuster);
            if (canTeleport)
                playerAnimator.ChangeAnimation(myAnimator, jump);
            else
                playerAnimator.ChangeAnimation(myAnimator, unchargedJump);
        }
        else if (inputManager.JumpHeld && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
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
        if (!canMove || dead || damaged || isTeleporting) return;

        if (inputManager.MoveInput.x < 0)
        {
            myBody.velocity = new Vector2(-MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
            if(myBody.velocity.y == 0 && !horizontalAttackArea.activeInHierarchy)
                if (canTeleport)
                    playerAnimator.ChangeAnimation(myAnimator, move);
                else
                    playerAnimator.ChangeAnimation(myAnimator, unchargedMove);
        }
        else if (inputManager.MoveInput.x > 0)
        {
            myBody.velocity = new Vector2(MoveSpeed + xVelocityAdjuster, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
            if(myBody.velocity.y == 0 && !horizontalAttackArea.activeInHierarchy)
                if (canTeleport)
                    playerAnimator.ChangeAnimation(myAnimator, move);
                else
                    playerAnimator.ChangeAnimation(myAnimator, unchargedMove);
        }
        else
        {
            myBody.velocity = new Vector2(xVelocityAdjuster, myBody.velocity.y);
            if(myBody.velocity.x == 0 && myBody.velocity.y <= 0 && !IsTeleporting && canAttack && !isJumping && isGrounded)
                if (canTeleport)
                    playerAnimator.ChangeAnimation(myAnimator, idle);
                else
                    playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);
        }
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>
    public void SpecialMove()
    {
        if (damaged || dead)
            return;

        if(inputManager.SpecialPressed && canTeleport)
        {
            isTeleporting = true;
            myBody.velocity = new Vector2(0, myBody.velocity.y);

            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            if (canTeleport)
                playerAnimator.ChangeAnimation(myAnimator, idle);
            else
                playerAnimator.ChangeAnimation(myAnimator, unchargedIdle);
        }
        else if(inputManager.SpecialHeld && canTeleport && isTeleporting)
        {
            TeleportSprites();
        }
        else if (inputManager.SpecialAction.WasReleasedThisFrame() && canTeleport && isTeleporting)
        {
            isTeleporting = false;
            foreach (SpriteRenderer sprite in teleportSprites)
            {
                sprite.enabled = false;
            }
            Teleport();
        }
    }

    private void TeleportSprites()
    {
        // Left and Up
        if (inputManager.MoveInput.x < 0 && inputManager.MoveInput.y > 0)
        {
            if (teleportSprites[0].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[0].enabled = true;
            }
            if (((transform.position.x - teleportDistance < negativeXBoundary) || (transform.position.y + teleportDistance > positiveYBoundary)) || teleportSprites[0].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Left and Down
        else if (inputManager.MoveInput.x < 0 && inputManager.MoveInput.y < 0)
        {
            if (teleportSprites[1].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[1].enabled = true;
            }
            if (((transform.position.x - teleportDistance < negativeXBoundary) || (transform.position.y - teleportDistance < negativeYBoundary)) || teleportSprites[1].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Right and Down
        else if (inputManager.MoveInput.x > 0 && inputManager.MoveInput.y < 0)
        {
            if (teleportSprites[2].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[2].enabled = true;
            }
            if (((transform.position.x + teleportDistance > positiveXBoundary) || (transform.position.y - teleportDistance < negativeYBoundary)) || teleportSprites[2].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Right and Up
        else if (inputManager.MoveInput.x > 0 && inputManager.MoveInput.y > 0)
        {
            if (teleportSprites[3].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[3].enabled = true;
            }
            if (((transform.position.x + teleportDistance > positiveXBoundary) || (transform.position.y + teleportDistance > positiveYBoundary)) || teleportSprites[3].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Left
        else if (inputManager.MoveInput.x < 0 && inputManager.MoveInput.y == 0)
        {
            if (teleportSprites[4].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[4].enabled = true;
            }
            if ((transform.position.x - teleportDistance < negativeXBoundary) || teleportSprites[4].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Up
        else if (inputManager.MoveInput.y > 0 && inputManager.MoveInput.x == 0)
        {
            if (teleportSprites[5].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[5].enabled = true;
            }
            if ((transform.position.y + teleportDistance > positiveYBoundary) || teleportSprites[5].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Down
        else if (inputManager.MoveInput.y < 0 && inputManager.MoveInput.x == 0)
        {
            if (teleportSprites[6].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[6].enabled = true; 
            }
            if ((transform.position.y - teleportDistance < negativeYBoundary) || teleportSprites[6].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Right
        else if (inputManager.MoveInput.x > 0 && inputManager.MoveInput.y == 0)
        {
            if (teleportSprites[7].enabled == false)
            {
                ResetTeleportSprites();
                teleportSprites[7].enabled = true;
            }
            if ((transform.position.x + teleportDistance > positiveXBoundary) || teleportSprites[7].GetComponent<Collider2D>().IsTouchingLayers(layermask))
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
        // Left and Up
        if((inputManager.MoveInput.x < 0 && inputManager.MoveInput.y > 0) || lastTeleportHeld == 1)
        {
            if(!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary) && !teleportSprites[0].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Left and Down
        else if((inputManager.MoveInput.x < 0 && inputManager.MoveInput.y < 0) || lastTeleportHeld == 2)
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary) && !teleportSprites[1].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Right and Down
        else if ((inputManager.MoveInput.x > 0 && inputManager.MoveInput.y < 0) || lastTeleportHeld == 3)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary) && !teleportSprites[2].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Right and Up
        else if ((inputManager.MoveInput.x > 0 && inputManager.MoveInput.y > 0) || lastTeleportHeld == 4)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary) && !teleportSprites[3].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Left
        else if (inputManager.MoveInput.x < 0 || lastTeleportHeld == 5)
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary) && !teleportSprites[4].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(-teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Up
        else if (inputManager.MoveInput.y > 0 || lastTeleportHeld == 6)
        {
            if (!(transform.position.y + teleportDistance > positiveYBoundary) && !teleportSprites[5].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(0, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Down
        else if (inputManager.MoveInput.y < 0 || lastTeleportHeld == 7)
        {
            if (!(transform.position.y - teleportDistance < negativeYBoundary) && !teleportSprites[6].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(0, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        // Right
        else if (inputManager.MoveInput.x > 0 || lastTeleportHeld == 8)
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !teleportSprites[7].GetComponent<Collider2D>().IsTouchingLayers(layermask))
            {
                CreateDyingProto();
                transform.position += new Vector3(teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        //Case for no teleport
        else
        {
            canMove = true;
            canJump = true;
        }

        lastTeleportHeld = 0;
        currentTPhelperTime = 0;
    }

    private void CreateDyingProto()
    {
        GameObject remnant = Instantiate(dyingProto, transform);
        remnant.transform.parent = transform.parent;
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
        if(!damaged && !dead)
        {
            canMove = true;
        }
        myBody.gravityScale = initialGravityModifier;

        yield return new WaitForSeconds(teleportCD - teleportHoldTime);

        canTeleport = true;
        DetermineCurrentChargedAnimation();
    }

    private void DetermineCurrentChargedAnimation()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedIdle.name))
            playerAnimator.ChangeAnimation(myAnimator, idle);
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedMove.name))
            playerAnimator.ChangeAnimation(myAnimator, move);
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedJump.name))
            playerAnimator.ChangeAnimation(myAnimator, jump);
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedAttackRight.name))
            playerAnimator.ChangeAnimation(myAnimator, attackRight);
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedAttackUp.name))
            playerAnimator.ChangeAnimation(myAnimator, attackUp);
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(unchargedHurt.name))
            playerAnimator.ChangeAnimation(myAnimator, hurt);
    }
    #endregion
}
