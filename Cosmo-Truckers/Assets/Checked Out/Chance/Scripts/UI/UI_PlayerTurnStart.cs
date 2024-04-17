using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerTurnStart : MonoBehaviour
{
    [SerializeField] SpriteRenderer attack;
    [SerializeField] SpriteRenderer aug;
    [SerializeField] SpriteRenderer intent;
    [SerializeField] SpriteRenderer attackOutline;
    [SerializeField] SpriteRenderer augOutline;
    [SerializeField] SpriteRenderer intentOutline;

    private void OnEnable()
    {
        ResetColor();
    }

    public void ResetColor()
    {
        attack.color = aug.color = intent.color = Color.white;
    }

    public void StartAttack()
    {
        attack.color = Color.white;
        aug.color = intent.color = Color.clear;

        attackOutline.color = Color.black;
        augOutline.color = intentOutline.color = Color.clear;
    }
    public void StartAUG()
    {
        aug.color = Color.white;
        attack.color = intent.color = Color.clear;

        augOutline.color = Color.black;
        attackOutline.color = intentOutline.color = Color.clear;
    }
    public void StartIntent()
    {
        intent.color = Color.white;
        attack.color = aug.color = Color.clear;

        intentOutline.color = Color.black;
        augOutline.color = attackOutline.color = Color.clear;
    }
}
