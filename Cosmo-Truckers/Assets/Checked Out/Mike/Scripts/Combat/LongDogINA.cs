using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogINA : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float stretchSpeed;
    [SerializeField] float stretchReturnSpeed;
    [SerializeField] float stretchRotateSpeed;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;

    bool stretching = false;
    bool canStretch = true; //make sure to set this to false ONLY when ass is retracting to skull
    bool canMove = true;

    PlayerCharacterINA INA;
    Rigidbody2D myBody;
    SpriteRenderer mySpriteHead;
    SpriteRenderer mySpriteBody;

    private void Start()
    {
        myBody = head.GetComponent<Rigidbody2D>();
        mySpriteHead = head.GetComponent<SpriteRenderer>();
        mySpriteBody = head.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Attack();
        Movement();
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
        }
    }

    #region Attack
    /// <summary>
    /// Long Dogs Attack will be his head being an active hitbox while he stetches. This attack function will handle the stretching
    /// </summary>
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canStretch)
        {
           if(stretching)
            {
                canMove = false;
                canStretch = false;
                //AssToHead();
            }
            else
            {
                stretching = true;
                body.transform.SetParent(transform);
                myBody.gravityScale = 0;
            }
        }
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

        if(stretching)
        {
            head.transform.Translate(Vector3.left * stretchSpeed * Time.deltaTime);

            if(transform.eulerAngles.y != 0)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
                {
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
                {
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
                {
                    head.transform.Rotate(new Vector3(0, 0, stretchRotateSpeed * Time.deltaTime));
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
                {
                    head.transform.Rotate(new Vector3(0, 0, -stretchRotateSpeed * Time.deltaTime));
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                transform.eulerAngles = new Vector3(0, 180, 0);
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

    }
    #endregion
}
