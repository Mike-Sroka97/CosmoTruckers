using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhipCenterBox : MonoBehaviour
{
    [SerializeField] Color OnColor;

    ElectroWhipEnemy currentEnemy;
    SpriteRenderer myChildSprite;
    Color startingColor;

    private void Start()
    {
        myChildSprite = GetComponentsInChildren<SpriteRenderer>()[1];
        startingColor = myChildSprite.color;
    }

    public void ActivateMe(ElectroWhipEnemy enemy)
    {
        currentEnemy = enemy;
        myChildSprite.color = OnColor;
    }

    private void DeactivateMe()
    {
        currentEnemy.Recenter();
        myChildSprite.color = startingColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack" && myChildSprite.color == OnColor)
        {
            DeactivateMe();
        }
    }
}
