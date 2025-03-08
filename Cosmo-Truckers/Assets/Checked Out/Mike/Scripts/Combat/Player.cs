using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 2.4f;
    [SerializeField] protected float jumpSpeed;
    [HideInInspector] public float xVelocityAdjuster;
    [HideInInspector] public float yVelocityAdjuster;

    [Space(20)]
    [Header("Damage Variables")]
    [SerializeField] protected float damageFlashSpeed;
    [SerializeField] protected float damagedDuration;
    [SerializeField] protected float iFrameDuration;
    [HideInInspector] public bool iFrames = false;
    [HideInInspector] public bool damaged = false;
    [HideInInspector] public bool dead = false;

    [Space(20)]
    [Header("Art")]
    [SerializeField] public SpriteRenderer[] MyRenderers;
    [SerializeField] Material iFrameMaterial;

    [Space(20)]
    [Header("Multiplayer")]
    public Material StartingMaterial;

    protected Rigidbody2D myBody;
    public PlayerCharacter MyCharacter;
    protected AudioDevice myAudioDevice;

    /// <summary>
    /// Public audio device getter
    /// </summary>
    public AudioDevice MyAudioDevice
    {
        get { return myAudioDevice; }
    }

    //Things that can be affected by buffs / debuffs
    protected float initialGravityModifier;

    protected void DebuffInit()
    {
        float startingGravity = myBody.gravityScale;
        initialGravityModifier = myBody.gravityScale * MyCharacter.Stats.Gravity; // * grav modifier debuff
        myBody.gravityScale = initialGravityModifier;

        if(myBody.gravityScale != startingGravity)
            jumpSpeed /= myBody.gravityScale - startingGravity;
    }

    public void PlayerInitialize()
    {
        if(!myBody)
            myBody = GetComponent<Rigidbody2D>();
        DebuffInit();

        myAudioDevice = GetComponentInChildren<AudioDevice>();
    }

    public void TakeDamage()
    {
        if(!iFrames)
        {
            myBody.velocity = Vector2.zero;
            damaged = true;
            iFrames = true;
            StartCoroutine(Damaged());
        }
    }

    public void Death()
    {
        myAudioDevice.PlaySound("Death"); 
    }

    protected void UpdateOutline()
    {
        //iFrame color outline
        if(iFrames)
        {
            foreach (SpriteRenderer renderer in MyRenderers)
            {
                renderer.material = iFrameMaterial;
            }
        }
        //else normal color outline
        else 
        {
            foreach (SpriteRenderer renderer in MyRenderers)
            {
                renderer.material = StartingMaterial;
            }
        }
    }

    public abstract IEnumerator Damaged();

    /// <summary>
    /// Set up each individual player for when a move ends <br></br>
    /// Used in training mode. 
    /// </summary>
    public virtual void EndMoveSetup()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        // If parent player exists, let it handle the movement still
        if (transform.parent.GetComponent<ParentPlayer>() == null)
        {
            if (body != null)
                body.velocity = new Vector2(0, body.velocity.y);

            else
            {
                body = GetComponentInChildren<Rigidbody2D>();
                body.velocity = new Vector2(0, body.velocity.y);
            }
        }

        enabled = false; 
    }
}
