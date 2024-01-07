using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 2.4f;
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
    [SerializeField] SpriteRenderer[] myRenderers;
    [SerializeField] Material iFrameMaterial; 

    protected Rigidbody2D myBody;
    private Material startingMaterial;
    public PlayerCharacter MyCharacter;

    //Things that can be affected by buffs / debuffs
    protected float initialGravityModifier;

    protected void DebuffInit()
    {
        initialGravityModifier = myBody.gravityScale; // * grav modifier debuff
    }

    public void PlayerInitialize()
    {
        myBody = GetComponent<Rigidbody2D>();
        startingMaterial = myRenderers[0].material; 
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

    protected void UpdateOutline()
    {
        //iFrame color outline
        if(iFrames)
        {
            foreach (SpriteRenderer renderer in myRenderers)
            {
                renderer.material = iFrameMaterial;
            }
        }
        //else normal color outline
        else 
        {
            foreach (SpriteRenderer renderer in myRenderers)
            {
                renderer.material = startingMaterial;
            }
        }
    }

    public abstract IEnumerator Damaged();
}
