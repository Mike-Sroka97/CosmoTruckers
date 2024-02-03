using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShockHittable : MonoBehaviour
{
    [SerializeField] Sprite disabledSprite;
    [SerializeField] Sprite enabledSprite;
    [SerializeField] Sprite hitSprite;
    [SerializeField] Material enabledMaterial;
    [SerializeField] Material disabledMaterial;
    [SerializeField] SpriteRenderer myRenderer;

    [HideInInspector] public bool Hit { get; private set; }

    Collider2D myCollider;
    SystemShock minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SystemShock>();
    }

    public void DeactivateMe()
    {
        myRenderer.sprite = disabledSprite;
        Hit = false;
    }

    public void ActivateMe()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer.sprite = enabledSprite;
        myRenderer.material = enabledMaterial;
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);

        if (collision.tag == "PlayerAttack" && !Hit)
        {
            minigame.AugmentScore++;
            minigame.GetComponent<SystemShock>().HittablesHit++;
            Hit = true;
            myRenderer.sprite = hitSprite;
            myCollider.enabled = false;
            myRenderer.material = disabledMaterial; 
        }
    }
}
