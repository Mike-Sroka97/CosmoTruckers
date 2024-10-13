using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyGloomGuy : EventNodeBase
{
    [SerializeField] int dissolveDamage = 15;

    public override void Initialize(EventNodeHandler handler)
    {
        base.Initialize(handler);
        CheckDissolveStatus();
    }

    public void SwallowTheGloop()
    {
        currentCharacter.AddDebuffStack(augmentsToAdd[0]);
        IteratePlayerReference();
        CheckDissolveStatus();
        AutoSelectNextButton();
        CheckEndEvent();
    }

    public void Dissolve()
    {
        currentCharacter.TakeDamage(dissolveDamage);
        IteratePlayerReference();
        CheckDissolveStatus();
        AutoSelectNextButton();
        CheckEndEvent();
    }

    private void CheckDissolveStatus()
    {
        if (currentCharacter.CurrentHealth <= dissolveDamage)
            myButtons[1].enabled = false;
        else
            myButtons[1].enabled = true;
    }
}
