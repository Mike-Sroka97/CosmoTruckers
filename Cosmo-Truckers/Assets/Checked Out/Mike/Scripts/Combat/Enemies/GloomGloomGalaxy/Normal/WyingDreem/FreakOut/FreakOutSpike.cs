using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOutSpike : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] float moveSpeed;

    bool isMoving;

    SpriteRenderer myRenderer;
    Animator myAnimator;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
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
        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, moveSpeed * Time.deltaTime);
    }

    public void RemoveOutline()
    {
        myRenderer.material = defaultMaterial;
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
