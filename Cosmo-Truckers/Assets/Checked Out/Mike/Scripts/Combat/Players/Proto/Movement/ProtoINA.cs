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

    [SerializeField] float teleportDistance;
    [SerializeField] float teleportCD;

    [SerializeField] float positiveXBoundary;
    [SerializeField] float positiveYBoundary;
    [SerializeField] float negativeXBoundary;
    [SerializeField] float negativeYBoundary;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canTeleport = true;

    float currentJumpStrength;
    float currentJumpHoldTime = 0;

    Collider2D myCollider;
    int layermask = 1 << 9;
    SpriteRenderer myRenderer;

    private void Start()
    {
        currentJumpStrength = jumpSpeed;
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponentsInChildren<Collider2D>()[0];
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
        SpecialMove();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
            currentJumpHoldTime = 0;
            currentJumpStrength = jumpSpeed;
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
        }
    }
    public override IEnumerator Damaged()
    {
        float damagedTime = 0;

        while (damagedTime < iFrameDuration)
        {
            myRenderer.enabled = !myRenderer.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                damaged = false;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
        myRenderer.enabled = true;
    }

    private void IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + .05f, layermask))
        {
            canJump = true;
            if(!isJumping)
            {
                currentJumpHoldTime = 0;
            }
        }
        else
        {
            canJump = false;
        }
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
        if (Input.GetKeyDown(KeyCode.Mouse1) && canTeleport)
        {
            Teleport();
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
        canTeleport = false;
        yield return new WaitForSeconds(teleportCD);
        canTeleport = true;
    }

    #endregion
}
