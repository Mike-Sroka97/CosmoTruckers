using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTetherEnemy : MonoBehaviour
{
    [SerializeField] GameObject deathParticles; 

    //Movement
    [SerializeField] float followSpeed;
    [SerializeField] float dashSpeed;

    //Attack
    [SerializeField] float attackCD;
    [SerializeField] float attackDistance;
    [SerializeField] Sprite notAttackingSprite;
    [SerializeField] Sprite attackingSprite;
    [SerializeField] Color attackingColor; 

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
    SpriteRenderer myRenderer;
    ParticleUpdater myParticleUpdater; 
    ProtoINA proto;
    Rigidbody2D myBody;
    Collider2D myCollider;
    Color notAttackingColor; 
    float currentTime = 0;
    float currentJumpTime = 0;
    const float distance = 0.05f;
    int layermask = 1 << 9;

    bool isAttacking;
    bool movingLeft;
    bool startDelay = true;
    bool initialized = false;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myParticleUpdater = GetComponentInChildren<ParticleUpdater>();
        myParticleUpdater.SetParticleState(false); 
        notAttackingColor = myRenderer.color; 
        minigame = FindObjectOfType<TripleTether>();
    }

    public void Intialize()
    {
        initialized = true;
        proto = FindObjectOfType<ProtoINA>();
    }

    private void Update()
    {
        if (!initialized)
            return;

        TrackRotation();
        Attack();
        MoveTowardsPlayer();
        TrackJump();
        SetCurrentTime(); 
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
        myRenderer.sprite = attackingSprite;
        myRenderer.color = attackingColor; 
        myBody.velocity = new Vector2(0, myBody.velocity.y);

        yield return new WaitForSeconds(attackDelay);

        myParticleUpdater.SetParticleState(true);

        if (movingLeft)
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
        myParticleUpdater.SetParticleState(false);
        myRenderer.sprite = notAttackingSprite;
        myRenderer.color = notAttackingColor; 
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
        minigame.Score++;
        minigame.CheckSuccess();
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation, minigame.transform);
        Destroy(gameObject);
    }

    private void SetCurrentTime()
    {
        if (Aggro)
            currentTime += Time.deltaTime;
        else
            currentTime = 0;
    }
}
