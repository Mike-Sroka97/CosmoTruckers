using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorIncubatorChoiceButton : MonoBehaviour
{
    [SerializeField] int scoreValue;

    AstorIncubation minigame;

    private void Start()
    {
        minigame = transform.parent.parent.GetComponent<AstorIncubation>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score = scoreValue;
        }
    }
}
