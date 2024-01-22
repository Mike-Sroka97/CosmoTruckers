using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhipEnemy : MonoBehaviour
{
    [Header("Leashed Attributes")]
    [SerializeField] Vector3 centerLinePoint;
    [SerializeField] float leashedSpeed;
    [SerializeField] float rotationLimit;
    [SerializeField] float rotationSpeed;
    [SerializeField] float breakDistance;

    [Space(20)]
    [Header("Unleashed Attributes")]
    [SerializeField] float unleashedSpeed;
    [SerializeField] int unleashedDirection; // 1 == x, y 2 == -x, y 3 == -x, -y 4 == x, -y

    [Space(20)]
    [Header("Releashed Attributes")]
    [SerializeField] float releashSpeed;
    [SerializeField] float threshold;

    [Space(20)]
    [Header("Material Attributes")]
    [SerializeField] Material selectedMaterial;
    SpriteRenderer mySpriteRenderer; 
    Material defaultMaterial;

    ElectroWhipCenterBox box;
    ElectroWhip minigame;
    Vector3 startingRotation;
    LineRenderer leash;
    Vector3[] leashPoints;
    int layermask = 1 << 9; //ground
    Rigidbody2D myBody;
    Collider2D myCollider;
    const float distance = 0.05f;
    Vector3 startingPos;

    bool releashing = false;
    bool rotationLeft = true;
    bool isLeashed = true;
    bool overLoadedPositiveRotation = false;
    bool overLoadedNegativeRotation = false;
    bool active = false;

    private void Start()
    {
        //rotation parameters
        startingRotation = transform.eulerAngles;
        int coinFlip = UnityEngine.Random.Range(0, 2);
        if (coinFlip == 1)
            rotationLeft = false;

        if(startingRotation.z + rotationLimit > 360)
        {
            overLoadedPositiveRotation = true;
        }
        if(startingRotation.z - rotationLimit < 0)
        {
            overLoadedNegativeRotation = true;
        }

        //other init stuff
        leash = GetComponentInChildren<LineRenderer>();
        leashPoints = new Vector3[2];
        leashPoints[0] = centerLinePoint;
        minigame = FindObjectOfType<ElectroWhip>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        box = FindObjectOfType<ElectroWhipCenterBox>();
        startingPos = transform.localPosition;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = mySpriteRenderer.material; 

        UpdateLeash();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && !releashing)
        {
            mySpriteRenderer.material = selectedMaterial; 
            box.ActivateMe(this);
        }
    }

    public void Initialize()
    {
        active = true;
    }

    private void Update()
    {
        UpdateLeash();

        if (!active)
            return;

        //Unleashed Stuff
        CheckLeash();
        UnleashedMovement();

        //Leash Stuff
        LeashRotation();
        LeashMovement();
    }

    private void CheckLeash()
    {
        if(Vector3.Distance(leashPoints[0], leashPoints[1]) > breakDistance && isLeashed)
        {
            minigame.Score--;
            isLeashed = false;
            leash.enabled = false;
            StartingVelocity();
        }
    }

    private void StartingVelocity()
    {
        switch (unleashedDirection)
        {
            case 1:
                myBody.velocity = new Vector2(unleashedSpeed, unleashedSpeed);
                break;
            case 2:
                myBody.velocity = new Vector2(-unleashedSpeed, unleashedSpeed);
                break;
            case 3:
                myBody.velocity = new Vector2(-unleashedSpeed, -unleashedSpeed);
                break;
            case 4:
                myBody.velocity = new Vector2(unleashedSpeed, -unleashedSpeed);
                break;
            default:
                break;
        }

    }
    private bool GroundCheck(Vector2 direction, bool horizontal)
    {
        if (horizontal)
        {
            return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.x + distance, layermask);
        }
        return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.y + distance, layermask);
    }

    private void UnleashedMovement()
    {
        if (releashing)
        {
            myBody.velocity = Vector2.zero;
            return;
        }

        else if (!isLeashed)
        {
            if (GroundCheck(Vector2.up, false))
            {
                myBody.velocity = new Vector2(myBody.velocity.x, -unleashedSpeed);
            }
            if (GroundCheck(Vector2.down, false))
            {
                myBody.velocity = new Vector2(myBody.velocity.x, unleashedSpeed);
            }
            if (GroundCheck(Vector2.right, true))
            {
                myBody.velocity = new Vector2(-unleashedSpeed, myBody.velocity.y);
            }
            if (GroundCheck(Vector2.left, true))
            {
                myBody.velocity = new Vector2(unleashedSpeed, myBody.velocity.y);
            }
        }
    }

    private void LeashRotation()
    {
        if (!isLeashed && !releashing)
            return;

        //the magic 10s are just a buffer. SUE ME
        if(rotationLeft)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            if(overLoadedPositiveRotation)
            {
                if (transform.eulerAngles.z > startingRotation.z + rotationLimit - 360 && transform.eulerAngles.z < startingRotation.z + rotationLimit - 360 + 10)
                {
                    rotationLeft = false;
                }
            }
            else if(transform.eulerAngles.z > startingRotation.z + rotationLimit && transform.eulerAngles.z < startingRotation.z + rotationLimit + 10)
            {
                rotationLeft = false;
            }
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
            if(overLoadedNegativeRotation)
            {
                if(transform.eulerAngles.z < 360 - Mathf.Abs(startingRotation.z - rotationLimit) && transform.eulerAngles.z > 360 - Mathf.Abs(startingRotation.z - rotationLimit) - 10)
                {
                    rotationLeft = true;
                }
            }
            else if (transform.eulerAngles.z < startingRotation.z - rotationLimit && transform.eulerAngles.z > startingRotation.z - rotationLimit - 10)
            {
                rotationLeft = true;
            }
        }
    }

    private void LeashMovement()
    {
        if (!isLeashed && ! releashing)
            return;

        transform.Translate(Vector2.up * leashedSpeed * Time.deltaTime);
    }
    
    private void UpdateLeash()
    {
        leashPoints[1] = transform.position;
        leash.SetPositions(leashPoints);
    }

    public void Recenter()
    {
        if(!releashing)
        {
            StartCoroutine(Releash());
        }
    }

    IEnumerator Releash()
    {
        releashing = true;

        while (Vector3.Distance(transform.localPosition, startingPos) > threshold)
        {
            // Calculate the direction towards the target position
            Vector3 direction = (startingPos - transform.localPosition).normalized;

            // Move towards the target position
            transform.position += direction * releashSpeed * Time.deltaTime;

            yield return null;
        }

        if(!isLeashed)
            minigame.Score++;

        releashing = false;
        isLeashed = true;
        leash.enabled = true;
        ResetThisEnemy();
    }

    public void ResetThisEnemy()
    {
        mySpriteRenderer.material = defaultMaterial; 
    }
}
