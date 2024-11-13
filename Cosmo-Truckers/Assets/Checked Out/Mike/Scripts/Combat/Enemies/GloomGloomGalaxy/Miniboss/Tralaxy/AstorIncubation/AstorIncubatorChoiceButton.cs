using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubatorChoiceButton : MonoBehaviour
{
    [SerializeField] int scoreValue;
    [SerializeField] int ballNumber;
    [SerializeField] Material toggleMaterial;
    [SerializeField] Sprite toggledSprite; 

    AstorIncubation minigame;
    SpriteRenderer myRenderer;
    Material defaultMaterial;
    Sprite defaultSprite; 

    private void Start()
    {
        minigame = FindObjectOfType<AstorIncubation>();
        myRenderer= GetComponent<SpriteRenderer>();

        defaultMaterial = myRenderer.material;
        defaultSprite = myRenderer.sprite;
    }

    private void Update()
    {
        if (minigame.platformsDisabled)
            NotPressed(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.ButtonInteraction(scoreValue, ballNumber, this);
        }
    }

    public void Pressed()
    {
        myRenderer.material = toggleMaterial;
        myRenderer.sprite = toggledSprite; 
    }

    public void NotPressed()
    {
        myRenderer.material = defaultMaterial;
        myRenderer.sprite = defaultSprite; 
    }
}
