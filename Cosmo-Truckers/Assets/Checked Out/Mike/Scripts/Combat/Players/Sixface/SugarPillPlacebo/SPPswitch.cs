using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SPPswitch : MonoBehaviour
{
    [SerializeField] GameObject myDefaultDeadZone;
    [SerializeField] SPPlayoutGenerator mySpawnZone;
    [SerializeField] Material toggledMaterial;
    [SerializeField] Sprite toggledSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject moneyParticle;
    [SerializeField] bool left;

    Collider2D myCollider;
    SpriteRenderer myRenderer;
    SugarPillPlacebo minigame; 

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        minigame = FindObjectOfType<SugarPillPlacebo>();

        if (minigame.CurrentSwitch != this)
            DisableMe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            Instantiate(moneyParticle, transform.position, Quaternion.identity, minigame.transform);
            myDefaultDeadZone.SetActive(false);
            DisableMe(); 
            mySpawnZone.GenerateLayout(left);
        }
    }

    public void ResetMe()
    {
        if (minigame.CurrentSwitch == this)
        {
            myDefaultDeadZone.SetActive(true);
            myCollider.enabled = true;
            myRenderer.material = defaultMaterial;
            myRenderer.sprite = defaultSprite;
        }
    }

    private void DisableMe()
    {
        myCollider.enabled = false;
        myRenderer.material = toggledMaterial;
        myRenderer.sprite = toggledSprite;
    }
}
