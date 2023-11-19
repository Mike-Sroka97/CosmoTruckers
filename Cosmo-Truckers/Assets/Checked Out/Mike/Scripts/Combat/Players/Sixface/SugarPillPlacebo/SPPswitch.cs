using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPswitch : MonoBehaviour
{
    [SerializeField] GameObject myDefaultDeadZone;
    [SerializeField] SPPlayoutGenerator mySpawnZone;
    [SerializeField] Material toggledMaterial;
    [SerializeField] Sprite toggledSprite; 

    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Material defaultMaterial;
    Sprite defaultSprite; 

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = myRenderer.material; 
        defaultSprite = myRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            myDefaultDeadZone.SetActive(false);
            myCollider.enabled = false;
            myRenderer.material = toggledMaterial;
            myRenderer.sprite = toggledSprite; 
            mySpawnZone.GenerateLayout();
        }
    }

    public void ResetMe()
    {
        myDefaultDeadZone.SetActive(true);
        myCollider.enabled = true;
        myRenderer.material = defaultMaterial;
        myRenderer.sprite = defaultSprite;
    }
}
