using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    //Movement variables
    [SerializeField] protected float moveSpeed;

    //Damaged variables
    [SerializeField] protected float damageFlashSpeed;
    [SerializeField] protected float damagedDuration;
    [SerializeField] protected float iFrameDuration;
    [HideInInspector] public bool iFrames = false;

    protected bool damaged = false;
    protected Rigidbody2D myBody;

    //Things that can be affected by buffs / debuffs
    protected float initialGravityModifier;

    protected void DebuffInit()
    {
        initialGravityModifier = myBody.gravityScale; // * grav modifier debuff
    }

    public void PlayerInitialize()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage()
    {
        myBody.velocity = Vector2.zero;
        damaged = true;
        iFrames = true;
        StartCoroutine(Damaged());
    }

    protected void UpdateOutline()
    {
        //if(iFrames(){}
        //else normal color outline
    }

    public abstract IEnumerator Damaged();

    public bool GetDamaged() { return damaged; }
}
