using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarStormBlock : MonoBehaviour
{
    [HideInInspector] bool IsBreakable = false;

    [SerializeField] Sprite breakableSprite;
    [SerializeField] Material breakableMaterial;

    SpriteRenderer myRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && IsBreakable)
        {
            Destroy(gameObject);
        }
    }

    public void ActivateMe()
    {
        IsBreakable = true;
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = breakableSprite;
        myRenderer.material = breakableMaterial;
    }
}
