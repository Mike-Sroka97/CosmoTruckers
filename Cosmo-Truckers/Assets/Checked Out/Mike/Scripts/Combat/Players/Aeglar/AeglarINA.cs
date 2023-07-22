using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarINA : Player
{
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCD;
    [SerializeField] float dashDuration;
    [SerializeField] float dashSlow;
    [SerializeField] float CDbetweenDashes = .25f;
    [SerializeField] float dashUpForce;

    //mech helpers
    public bool DashingLeft { get; private set; }
    public bool DashingRight { get; private set; }
    public bool DashingUp { get; private set; }

    [SerializeField] GameObject horizontalAttackArea;
    [SerializeField] GameObject verticalAttackArea;
    [SerializeField] int numberOfAttacks = 2;
    [SerializeField] int numberOfJumps = 2;

    [SerializeField] float jumpSpeed;

    bool canDash = true;
    bool canMove = true;

    Collider2D myCollider;
    int layermask = 1 << 9; //ground
    const float distance = 0.05f;
    int currentNumberOfAttacks = 0;
    int currentNumberOfJumps = 0;
    Vector2 bottomLeft;
    Vector2 bottomRight;

    private void Start()
    {
        PlayerInitialize();

        myBody = GetComponent<Rigidbody2D>();
        myCollider = transform.Find("AeglarBody").GetComponent<Collider2D>();
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
        bottomLeft = new Vector2(myCollider.bounds.min.x, myCollider.bounds.min.y);
        bottomRight = new Vector2(myCollider.bounds.max.x, myCollider.bounds.min.y);

        if (Physics2D.Raycast(bottomLeft, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(bottomRight, Vector2.down, myCollider.bounds.extents.y + distance, layermask)
            || Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask))
            return true;

        else return false;
    }

    public bool GetDashState() { return canDash; }

    public override IEnumerator Damaged()
    {
        float damagedTime = 0;
        canDash = false;
        canMove = false;

        while (damagedTime < iFrameDuration)
        {
            damagedTime += Time.deltaTime + damageFlashSpeed;
            if (damagedTime > damagedDuration)
            {
                canDash = true;
                canMove = true;
                damaged = false;
            }
            yield return new WaitForSeconds(damageFlashSpeed);
        }

        iFrames = false;
    }

    #region Attack
    /// <summary>
    /// Aeglar's attack will be a dash where he is an active hitbox during the process
    /// </summary>
    public void Attack()
    {
        if (currentNumberOfAttacks < numberOfAttacks)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDash && myBody.velocity.x < 0)
            {
                StartCoroutine(Dash(true));
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && canDash && myBody.velocity.x > 0)
            {
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
        if (IsGrounded(0.02f) && canDash)
        {
            currentNumberOfAttacks = 0;
            currentNumberOfJumps = 0;
        }

        if (Input.GetKeyDown("space") && currentNumberOfJumps < numberOfJumps)
        {
            StartCoroutine(Dash(true, true));
            ;
        }
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
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>

    IEnumerator Dash(bool left, bool up = false)
    {
        if (up)
        {
            currentNumberOfJumps++;
            verticalAttackArea.SetActive(true);
        }
        else
        {
            currentNumberOfAttacks++;
            horizontalAttackArea.SetActive(true);
        }

        canDash = false;
        canMove = false;

        myBody.velocity = Vector2.zero;

        if (up)
        {
            DashingUp = true;
            myBody.AddForce(new Vector2(0, jumpSpeed));
        }
        else if (left)
        {
            DashingLeft = true;
            myBody.AddForce(new Vector2(-dashSpeed, dashUpForce));
        }
        else
        {
            DashingRight = true;
            myBody.AddForce(new Vector2(dashSpeed, dashUpForce));
        }

        float currentDashTime = 0;
        while (currentDashTime < dashDuration)
        {
            if (up)
            {
                myBody.velocity = new Vector2(0, myBody.velocity.y);
            }
            else
            {
                myBody.velocity = new Vector2(myBody.velocity.x, 0);
            }

            currentDashTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        myBody.velocity = Vector2.zero;

        canMove = true;
        if (up)
        {
            verticalAttackArea.SetActive(false);
        }
        else
        {
            horizontalAttackArea.SetActive(false);
        }

        currentDashTime = 0;
        while (currentDashTime < CDbetweenDashes)
        {
            currentDashTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        DashingUp = false;
        DashingLeft = false;
        DashingRight = false;
        canDash = true;
    }
    public void SpecialMove()
    {

    }
    #endregion
}
