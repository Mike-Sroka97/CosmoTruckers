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
    [SerializeField] GameObject moneyParticle;

    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Material defaultMaterial;
    Sprite defaultSprite;
    TMP_Text timerText;
    CombatMove minigame; 

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = myRenderer.material; 
        defaultSprite = myRenderer.sprite;
        timerText = GetComponentInChildren<TMP_Text>();

        minigame = GameObject.FindObjectOfType<CombatMove>();
    }

    private void Update()
    {
        if (minigame.MoveEnded && timerText.gameObject.activeInHierarchy)
        {
            timerText.gameObject.SetActive(false); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {

            Instantiate(moneyParticle, transform.position, Quaternion.identity, minigame.transform);
            myDefaultDeadZone.SetActive(false);
            myCollider.enabled = false;
            myRenderer.material = toggledMaterial;
            myRenderer.sprite = toggledSprite; 
            mySpawnZone.GenerateLayout(timerText);
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
