using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] float attackDuration;
    [SerializeField] float attackCD;
    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject upAttackArea;

    [SerializeField] float jumpSpeedAccrual;
    [SerializeField] float jumpMaxHoldTime;

    [SerializeField] float hopForceModifier;
    [SerializeField] float raycastHopHelper;

    [SerializeField] float iFrameDuration;
    [SerializeField] float damageFlashSpeed;
    [SerializeField] float damagedDuration;

    bool canMove = true;
    bool canJump = true;
    bool isJumping = false;
    bool canAttack = true;
    bool damaged = false;
    [HideInInspector] public bool iFrames = false;

    float currentJumpStrength;
    float currentJumpHoldTime = 0;

    int layermask = 1 << 9; //ground

    Rigidbody2D myBody;
    SpriteRenderer mySprite;
    Collider2D myCollider;

    private void Start()
    {
        currentJumpStrength = 0;
        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = transform.Find("Body").GetComponent<Collider2D>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9 && IsGrounded(.02f))
        {
            ShortHop();
        }
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

        while (damagedTime < iFrameDuration)
        {
            mySprite.enabled = !mySprite.enabled;
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                damaged = false;
                ShortHop();
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W) && canAttack)
        {
            StartCoroutine(SafeTAttack(upAttackArea));
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
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
        if (Input.GetKeyDown("space") && canJump && !isJumping)
        {
            canMove = false;
            canJump = false;
            isJumping = true;
        }
        else if (Input.GetKey("space") && isJumping && currentJumpHoldTime < jumpMaxHoldTime)
        {
            currentJumpHoldTime += Time.deltaTime;
            if(currentJumpHoldTime > jumpMaxHoldTime) { currentJumpHoldTime = jumpMaxHoldTime; }
            currentJumpStrength = currentJumpHoldTime * jumpSpeedAccrual;
        }
        else if (isJumping && Input.GetKeyUp("space") && !Input.GetKey("space"))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, 0);
            myBody.AddForce(new Vector2(0, currentJumpStrength), ForceMode2D.Impulse);
            currentJumpHoldTime = 0;
            currentJumpStrength = 0;
            canMove = true;
            isJumping = false;
        }
    }

    private void ShortHop()
    {
        if(IsGrounded(raycastHopHelper))
        {
            if (!isJumping)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0);
                myBody.AddForce(new Vector2(0, hopForceModifier), ForceMode2D.Impulse);
            }
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private bool IsGrounded(float distance)
    {
        return Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask);
    }
    #endregion

    #region Movement
    /// <summary>
    /// Aeglar's movement will cause short spurts of movements. There is an associated cooldown to this
    /// </summary>
    public void Movement()
    {
        if (!canMove || damaged) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y == 0 && !horizontalAttackArea.activeInHierarchy)
            {
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
            if (transform.rotation.eulerAngles.y != 0 && !horizontalAttackArea.activeInHierarchy)
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
    /// Oops SafeT does not have a special move womp womp
    /// </summary>
    public void SpecialMove()
    {

    }
    #endregion
}
