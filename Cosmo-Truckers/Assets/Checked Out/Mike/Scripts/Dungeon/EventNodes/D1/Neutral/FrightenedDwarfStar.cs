using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedDwarfStar : EventNodeBase
{
    [SerializeField] int damage = 12;

    bool evilStar;

    protected override void Start()
    {
        base.Start();
        evilStar = MathHelpers.RandomBool();
    }

    public override void Initialize(EventNodeHandler handler)
    {
        base.Initialize(handler);
        if (currentCharacter.CurrentHealth <= damage)
            myButtons[0].enabled = false;
        else
            myButtons[0].enabled = true;
    }

    public void HelpTheStar()
    {
        if (evilStar)
            currentCharacter.TakeDamage(damage);
        else
            currentCharacter.DoubleAugment(1);

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }

    public override void IgnoreOption()
    {
        IteratePlayerReference();
        if (currentCharacter.CurrentHealth <= damage)
            myButtons[0].enabled = false;
        else
            myButtons[0].enabled = true;
        AutoSelectNextButton();
        CheckEndEvent();
    }
}
