using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixfaceINA : MonoBehaviour
{
    //Movement variables
    [SerializeField] float moveSpeed;

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

    //Hover variables
    [SerializeField] float hoverVelocityYMax;
    [SerializeField] float hoverGravityModifier;

    //Damaged variables
    [SerializeField] float damageFlashSpeed;
    [SerializeField] float damagedDuration;
    [SerializeField] float iFrameDuration;

    [HideInInspector] public bool IsHovering = false;
    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool canHover = true;
    bool damaged = false;
    [HideInInspector] public bool iFrames = false;

    float currentJumpHoldTime = 0;

    PlayerCharacterINA INA;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;
    Collider2D myCollider;
    int layermask = 1 << 9;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponentsInChildren<Collider2D>()[1]; //ignores parent
    }

    private void Update()
    {
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
        if(Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + .05f, layermask))
        {
            canJump = true;
            canHover = false;
            currentJumpHoldTime = 0;
        }
        else
        {
            canJump = false;
            canHover = true;
        }
    }

    public bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + .05f, layermask);
    }
    
    public void TakeDamage()
    {
        myBody.velocity = Vector2.zero;
        damaged = true;
        iFrames = true;
        StartCoroutine(Damaged());
    }

    IEnumerator Damaged()
    {
        float damagedTime = 0;

        while(damagedTime < iFrameDuration)
        {
            mySprite.enabled = !mySprite.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if(damagedTime > damagedDuration)
            {
                damaged = false;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
        mySprite.enabled = true;
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack)
        {
            StartCoroutine(SixFaceAttack(upAttackArea));
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.S) && canAttack && currentJumpHoldTime != 0)
        {
            StartCoroutine(SixFaceAttack(downAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            StartCoroutine(SixFaceAttack(horizontalAttackArea));
        }
    }

    IEnumerator SixFaceAttack(GameObject attack)
    {
        canAttack = false;
        attack.SetActive(true);
        attack.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attack.GetComponent<Collider2D>().enabled = false;
        attack.SetActive(false);
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
        IsGrounded();

        if(myBody.velocity.y > maxYvelocity)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, maxYvelocity);
        }

        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canJump = false;
            isJumping = true;
            myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y + (jumpSpeed * Time.deltaTime));
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y + (jumpSpeed * Time.deltaTime));
        }
        else if (isJumping)
        {
            isJumping = false;
            canHover = true;
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
        }
        else
        {
            IsHovering = false;
            myBody.gravityScale = 1;
        }
    }
    #endregion
}
