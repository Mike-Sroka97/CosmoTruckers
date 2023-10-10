using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubatorChoiceButton : MonoBehaviour
{
    [SerializeField] int scoreValue;
    [SerializeField] int ballNumber;

    AstorIncubation minigame; 

    private void Start()
    {
        minigame = transform.parent.parent.parent.GetComponent<AstorIncubation>();
    }

    private void Update()
    {
        if (minigame.platformsDisabled)
        {
            GetComponent<SpriteRenderer>().material = minigame.GetDefaultMaterial(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.ButtonInteraction(scoreValue, ballNumber, GetComponent<SpriteRenderer>());
        }
    }
}
