using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerTurnStart : MonoBehaviour
{
    [SerializeField] SpriteRenderer attack;
    [SerializeField] SpriteRenderer aug;
    [SerializeField] SpriteRenderer intent;

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
        attack.color = new Color(1, 1, 1, .4f);
        aug.color = intent.color = Color.white;
    }
    public void StartAUG()
    {
        aug.color = new Color(1, 1, 1, .4f);
        attack.color = intent.color = Color.white;
    }
    public void StartIntent()
    {
        intent.color = new Color(1, 1, 1, .4f);
        attack.color = aug.color = Color.white;
    }
}
