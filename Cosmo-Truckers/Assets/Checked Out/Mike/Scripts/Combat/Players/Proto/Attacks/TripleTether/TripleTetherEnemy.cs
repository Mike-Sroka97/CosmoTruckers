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

    [HideInInspector] public bool Aggro;


    TripleTether minigame;
    ProtoINA proto;
    Rigidbody2D myBody;
    float currentTime = 0;

    bool isAttacking;
    bool movingLeft;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        minigame = FindObjectOfType<TripleTether>();
        proto = FindObjectOfType<ProtoINA>();
    }

    private void Update()
    {
        TrackRotation();
        Attack();
        MoveTowardsPlayer();
        currentTime += Time.deltaTime;
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
        myBody.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackDelay);

        if(movingLeft)
        {
            myBody.velocity = new Vector2(-dashSpeed, 0);
        }
        else
        {
            myBody.velocity = new Vector2(dashSpeed, 0);
        }

        yield return new WaitForSeconds(attackDuration);

        myBody.velocity = Vector2.zero;

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
                myBody.velocity = new Vector2(-followSpeed, 0);
            }
            else
            {
                myBody.velocity = new Vector2(followSpeed, 0);
            }
        }
        else if(!Aggro)
        {
            myBody.velocity = Vector2.zero; 
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
