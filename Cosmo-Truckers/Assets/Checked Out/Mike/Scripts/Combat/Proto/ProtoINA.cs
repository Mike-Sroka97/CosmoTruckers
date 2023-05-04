using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject attackArea;

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

    PlayerCharacterINA INA;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    private void Start()
    {
        currentJumpStrength = jumpSpeed;
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
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

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(ProtoAttack());
        }
    }

    IEnumerator ProtoAttack()
    {
        canAttack = false;
        attackArea.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackArea.SetActive(false);
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
        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.AddForce(new Vector2(0, jumpSpeed));
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            currentJumpStrength = jumpSpeed * ((jumpMaxHoldTime - currentJumpHoldTime) / jumpMaxHoldTime);
            myBody.AddForce(new Vector2(0, currentJumpStrength));
        }
        else if (isJumping)
        {
            isJumping = false;
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

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) && !attackArea.activeInHierarchy)
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) && !attackArea.activeInHierarchy)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            if (transform.rotation.eulerAngles.y != 0)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
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
