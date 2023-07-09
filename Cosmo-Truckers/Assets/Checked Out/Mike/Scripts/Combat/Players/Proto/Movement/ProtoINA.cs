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

    [SerializeField] float positiveXBoundary;
    [SerializeField] float positiveYBoundary;
    [SerializeField] float negativeXBoundary;
    [SerializeField] float negativeYBoundary;

    [HideInInspector] public bool IsTeleporting { get; private set; }
    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canTeleport = true;

    float currentJumpHoldTime = 0;
    float currentCoyoteTime = 0;

    Collider2D myCollider;
    int layermask = 1 << 9;
    const float distance = 0.05f;
    SpriteRenderer myRenderer;
    Vector2 bottomLeft;
    Vector2 bottomRight;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponentsInChildren<Collider2D>()[0];
        myRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        DebuffInit();
    }

    private void Update()
    {
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

        myBody.velocity = Vector2.zero;
        canJump = false;
        canAttack = false;
        canMove = false;
        canTeleport = false;
        float damagedTime = 0;

        while (damagedTime < iFrameDuration)
        {
            myRenderer.enabled = !myRenderer.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                damaged = false;
                canAttack = true;
                canMove = true;
                canTeleport = true;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
        myRenderer.enabled = true;
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
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
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

        if(horizontal)
        {
            horizontalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            horizontalAttackArea.SetActive(false);
            yield return new WaitForSeconds(attackCD);
        }
        else
        {
            verticalAttackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            verticalAttackArea.SetActive(false);
            yield return new WaitForSeconds(attackCD);
        }

        canAttack = true;
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
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);
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
        else
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
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
        }
        else
        {
            ResetTeleportSprites();
        }
    }

    private void ResetTeleportSprites()
    {
        foreach(SpriteRenderer sprite in teleportSprites)
        {
            sprite.color = teleportSpriteStartingColor;
            sprite.enabled = false;
        }
    }

    private void Teleport()
    {
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            if(!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary))
            {
                transform.position += new Vector3(-teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary))
            {
                transform.position += new Vector3(-teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y - teleportDistance < negativeYBoundary))
            {
                transform.position += new Vector3(teleportDistance, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary) && !(transform.position.y + teleportDistance > positiveYBoundary))
            {
                transform.position += new Vector3(teleportDistance, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!(transform.position.x - teleportDistance < negativeXBoundary))
            {
                transform.position += new Vector3(-teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (!(transform.position.y + teleportDistance > positiveYBoundary))
            {
                transform.position += new Vector3(0, teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (!(transform.position.y - teleportDistance < negativeYBoundary))
            {
                transform.position += new Vector3(0, -teleportDistance, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!(transform.position.x + teleportDistance > positiveXBoundary))
            {
                transform.position += new Vector3(teleportDistance, 0, 0);
                StartCoroutine(TeleportCooldown());
            }
        }
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
