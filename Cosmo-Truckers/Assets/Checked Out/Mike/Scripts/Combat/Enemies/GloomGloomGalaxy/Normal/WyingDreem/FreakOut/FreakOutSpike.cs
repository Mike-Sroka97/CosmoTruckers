using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOutSpike : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] float moveSpeed;

    bool isMoving;
    Material hurtMaterial; 
    SpriteRenderer myRenderer;
    Animator myAnimator;
    CombatMove minigame; 

    public void Initialize()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        hurtMaterial = myRenderer.material;
        myAnimator = GetComponent<Animator>();
        minigame = FindObjectOfType<CombatMove>(); 
    }

    private void Update()
    {
        if(isMoving)
        {
            MoveMe();
        }
    }

    private void MoveMe()
    {
        transform.position = Vector2.MoveTowards(transform.position, minigame.transform.position, moveSpeed * Time.deltaTime);
    }

    public void SetOutline(bool hurt)
    {
        if (hurt) 
        { 
            myRenderer.material = hurtMaterial;
            myRenderer.color = new Color(1, 1, 1, 1); 
        }
        else 
        { 
            myRenderer.material = defaultMaterial;
            myRenderer.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SetMoving()
    {
        isMoving = true;
        myAnimator.Play("SpikeDestroy");
    }
    
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
