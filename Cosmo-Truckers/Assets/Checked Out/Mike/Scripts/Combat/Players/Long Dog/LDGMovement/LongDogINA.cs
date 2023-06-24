using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogINA : Player
{
    [SerializeField] float stretchSpeed;
    [SerializeField] float stretchReturnSpeed;
    [SerializeField] float stretchRotateSpeed;
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

    LongDogNeck currentLine;

    Camera cam;

    bool stretching = false;
    bool buttStretching = false;
    bool canStretch = true; //make sure to set this to false ONLY when ass is retracting to skull
    bool canMove = true;
    bool invincible = false;
    bool goingLeft = false;
    bool goingRight = false;
    bool startupStretch = false;

    Vector3 buttStartingLocation;
    SpriteRenderer mySpriteHead;
    SpriteRenderer mySpriteBody;
    float startingGravityScale;
    int layermask = 1 << 9;
    Collider2D myCollider;

    private void Start()
    {
        PlayerInitialize();

        myBody = head.GetComponent<Rigidbody2D>();
        mySpriteHead = head.GetComponent<SpriteRenderer>();
        mySpriteBody = head.GetComponentInChildren<SpriteRenderer>();
        myCollider = head.GetComponent<Collider2D>();
        buttStartingLocation = body.transform.localPosition;
        cam = Camera.main;
        startingGravityScale = myBody.gravityScale;
    }

    private void Update()
    {
        if(!stretching && canMove && damaged && !invincible)
        {
            canMove = false;
            canStretch = false;
            StartCoroutine(Damaged());
        }
        Attack();
        Movement();
        Jump();
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
            if (collision == "EnemyDamaging" && !damaged)
            {
                damaged = true;
            }
        }
    }

    public override IEnumerator Damaged()
    {
        float damagedTime = 0;
        mySpriteBody = body.GetComponent<SpriteRenderer>();
        myBody.velocity = Vector2.zero;

        while(damagedTime < iFrameDuration)
        {
            if(FindObjectOfType<LongDogNeck>())
            {
                LongDogNeck tempNeck = FindObjectOfType<LongDogNeck>();
                tempNeck.GetComponent<LineRenderer>().enabled = false; 
            }
            mySpriteHead.enabled = false;
            mySpriteBody.enabled = false;
            yield return new WaitForSeconds(damageFlashSpeed);

            if (FindObjectOfType<LongDogNeck>())
            {
                LongDogNeck tempNeck = FindObjectOfType<LongDogNeck>();
                tempNeck.GetComponent<LineRenderer>().enabled = true;
            }
            mySpriteHead.enabled = true;
            mySpriteBody.enabled = true;
            yield return new WaitForSeconds(damageFlashSpeed);

            damagedTime += Time.deltaTime + (damageFlashSpeed * 2);
            if(damagedTime >= damagedDuration && !invincible)
            {
                invincible = true;
                damaged = false;
                LDGReset();
            }
        }
        invincible = false;
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
        stretching = true;
        myNose.enabled = true;
        body.transform.SetParent(transform);
        myBody.gravityScale = 0;

        currentLine = Instantiate(linePrefab, this.transform).GetComponent<LongDogNeck>();

        currentLine.SetLineColor(lineColor);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }

    void Draw()
    {
        currentLine.AddPoint(head.transform.localPosition);
    }
    void EndDraw()
    {
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

    IEnumerator ATHSpin()
    {
        invincible = true;
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
        invincible = false;
        if (damaged && !invincible)
        {
            StartCoroutine(Damaged());
        }
        else
        {
            LDGReset();
        }
    }

    public void LDGReset()
    {
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
            goingLeft = false;
            goingRight = false;

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
            {
                myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
                head.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
            {
                myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
                head.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if(IsGrounded())
            {
                myBody.velocity = new Vector2(0, myBody.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(head.transform.position, Vector2.down, myCollider.bounds.extents.y + .05f, layermask))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region SpecialMove
    /// <summary>
    /// Aeglar's attack will also make him dash. That will bundle the Special move and attack for him
    /// </summary>
    public void SpecialMove()
    {

    }
    #endregion
}
