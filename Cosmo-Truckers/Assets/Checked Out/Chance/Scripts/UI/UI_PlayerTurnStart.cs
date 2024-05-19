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
    [SerializeField] GameObject[] disabledIcons;

    [HideInInspector] public bool AttackDisabled;
    [HideInInspector] public bool AugDisabled;
    [HideInInspector] public bool InsightDisabled;

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
        attackOutline.color = Color.clear;
    }

    public void DisableButton(int buttonToDisable)
    {
        if(buttonToDisable == 1)
        {
            AttackDisabled = true;
            disabledIcons[0].SetActive(true);
        }
        else if(buttonToDisable == 2)
        {
            AugDisabled = true;
            disabledIcons[1].SetActive(true);
        }
        else
        {
            InsightDisabled = true;
            disabledIcons[2].SetActive(true);
        }
    }

    public void EnableButton(int buttonToEnable)
    {
        if (buttonToEnable == 1)
        {
            AttackDisabled = false;
            disabledIcons[0].SetActive(false);
        }
        else if (buttonToEnable == 2)
        {
            AugDisabled = false;
            disabledIcons[1].SetActive(false);
        }
        else
        {
            InsightDisabled = false;
            disabledIcons[2].SetActive(false);
        }
    }

    public void EnableAllButtons()
    {
        AttackDisabled = false;
        AugDisabled = false;
        InsightDisabled = false;
        disabledIcons[0].SetActive(false);
        disabledIcons[1].SetActive(false);
        disabledIcons[2].SetActive(false);
    }
}
