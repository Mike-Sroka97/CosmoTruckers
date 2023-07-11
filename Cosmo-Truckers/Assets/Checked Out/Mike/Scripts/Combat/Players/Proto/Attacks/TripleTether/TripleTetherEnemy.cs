using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTetherEnemy : MonoBehaviour
{
    [SerializeField] int health;

    //Movement
    [SerializeField] float followSpeed;
    [SerializeField] float dashSpeed;

    //Attack
    [SerializeField] float attackCD;
    [SerializeField] float attackDistance;
    [SerializeField] Sprite notAttackingSprite;
    [SerializeField] Sprite attackingSprite;
    [SerializeField] SpriteRenderer face;

    //coroutine stuff
    [SerializeField] float attackDelay;
    [SerializeField] float attackDuration;
    [SerializeField] float attackEndDelay;

    //jump stuff
    [SerializeField] float jumpDelay;
    [SerializeField] float jumpForce;
    [SerializeField] GameObject shockwave;
    [SerializeField] Transform shockSpawn;

    [HideInInspector] public bool Aggro;


    TripleTether minigame;
    ProtoINA proto;
    Rigidbody2D myBody;
    Collider2D myCollider;
    float currentTime = 0;
    float currentJumpTime = 0;
    const float distance = 0.05f;
    int layermask = 1 << 9;

    bool isAttacking;
    bool movingLeft;
    bool startDelay = true;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        minigame = FindObjectOfType<TripleTether>();
        proto = FindObjectOfType<ProtoINA>();
    }

    private void Update()
    {
        TrackRotation();
        Attack();
        MoveTowardsPlayer();
        TrackJump();
        currentTime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            minigame.PlayerDead = true;
        }
        else if(Physics2D.Raycast(transform.position, Vector2.down, myCollider.bounds.extents.y + distance, layermask) && !startDelay)
        {
            GameObject shock = Instantiate(shockwave, shockSpawn);
            shock.transform.parent = null;
        }
    }

    private void TrackJump()
    {
        if(Aggro && !isAttacking)
        {
            currentJumpTime += Time.deltaTime;
            if(currentJumpTime >= jumpDelay)
            {
                startDelay = false;
                currentJumpTime = 0;
                myBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    private void TrackRotation()
    {
        if (isAttacking || !Aggro)
            return;

        if (proto.transform.position.x < transform.position.x)
        {
            movingLeft = true;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            movingLeft = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void Attack()
    {
        if (!Aggro)
            return;

        if(Vector2.Distance(proto.transform.position, transform.position) < attackDistance && !isAttacking && currentTime >= attackCD)
        {
            StartCoroutine(AttackCo());
        }
    }

    IEnumerator AttackCo()
    {
        isAttacking = true;
        face.sprite = attackingSprite;
        myBody.velocity = new Vector2(0, myBody.velocity.y);

        yield return new WaitForSeconds(attackDelay);

        if(movingLeft)
        {
            myBody.velocity = new Vector2(-dashSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(dashSpeed, myBody.velocity.y);
        }

        yield return new WaitForSeconds(attackDuration);

        myBody.velocity = new Vector2(0, myBody.velocity.y); 

        yield return new WaitForSeconds(attackEndDelay);

        isAttacking = false;
        face.sprite = notAttackingSprite;
        currentTime = 0;
    }

    private void MoveTowardsPlayer()
    {
        if(!isAttacking && Aggro)
        {
            if (movingLeft)
            {
                myBody.velocity = new Vector2(-followSpeed, myBody.velocity.y);
            }
            else
            {
                myBody.velocity = new Vector2(followSpeed, myBody.velocity.y);
            }
        }
        else if(!Aggro)
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }
    }

    public void TakeDamage()
    {
        health--;
        minigame.Score++;
        Debug.Log(minigame.Score);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
