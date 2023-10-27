using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPhittable : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float distance;
    [SerializeField] float moveSpeedIncrease;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] Sprite hitSprite; 
    [SerializeField] float timeBetweenSprites = 0.2f;
    [SerializeField] Material hurtMaterial;

    Material startingMaterial; 
    PowerPummel minigame;
    Rigidbody2D myBody;
    Collider2D myCollider;
    Sprite originalSprite; 
    SpriteRenderer myRenderer; 
    int layermask = 1 << 9; //ground
    bool isHit = false; 

    private void Start()
    {
        GetStartingVariables();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            if (!isHit)
            {
                myBody.velocity = -myBody.velocity;
                UpdateSpeed();
                StartCoroutine(UpdateSprite());
                minigame.Score++;
                Debug.Log(minigame.Score);
                minigame.CheckScore(); 
            }
        }
    }

    private void GetStartingVariables()
    {
        myBody = GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<PowerPummel>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        originalSprite = myRenderer.sprite;
        startingMaterial = myRenderer.material;
    }

    private void Update()
    {
        DetermineDirection();
    }

    private void UpdateSpeed()
    {
        if(myBody.velocity.x > maxMoveSpeed)
        {
            ChangeSpeed(maxMoveSpeed);
        }
        else
        {
            moveSpeed += moveSpeedIncrease;
            ChangeSpeed(moveSpeed);
        }
    }

    private IEnumerator UpdateSprite()
    {
        isHit = true; 
        myRenderer.sprite = hitSprite;
        myRenderer.material = hurtMaterial; 
        yield return new WaitForSeconds(timeBetweenSprites);
        myRenderer.sprite = originalSprite;
        myRenderer.material = startingMaterial; 
        isHit = false; 
    }

    private void ChangeSpeed(float speed)
    {
        float xSpeed = 0;
        float ySpeed = 0;

        if(myBody.velocity.x > 0)
        {
            xSpeed = speed;
        }
        else
        {
            xSpeed = -speed;
        }
        if(myBody.velocity.y > 0)
        {
            ySpeed = speed;
        }
        else
        {
            ySpeed = -speed;
        }

        myBody.velocity = new Vector2(xSpeed, ySpeed);
    }

    private void DetermineDirection()
    {
        if(GroundCheck(Vector2.up, false))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeed);
        }
        if(GroundCheck(Vector2.down, false))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, moveSpeed);
        }
        if (GroundCheck(Vector2.right, true))
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        if (GroundCheck(Vector2.left, true))
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
    }

    private bool GroundCheck(Vector2 direction, bool horizontal)
    {
        if(horizontal)
        {
            return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.x + distance, layermask);
        }
        return Physics2D.Raycast(transform.position, direction, myCollider.bounds.extents.y + distance, layermask);
    }

    public void DetermineStartingMovement()
    {
        if (myBody == null)
        {
            GetStartingVariables();
        }

        int rightRandom = UnityEngine.Random.Range(0, 2); //coin flip bb
        int upRandom = UnityEngine.Random.Range(0, 2); //coin flip bb

        if (rightRandom == 0)
        {
            myBody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myBody.velocity = new Vector2(-moveSpeed, 0);
        }
        if (upRandom == 0)
        {
            myBody.velocity = new Vector2(myBody.velocity.x, moveSpeed);
        }
        else
        {
            myBody.velocity = new Vector2(myBody.velocity.x, -moveSpeed);
        }
    }
}
