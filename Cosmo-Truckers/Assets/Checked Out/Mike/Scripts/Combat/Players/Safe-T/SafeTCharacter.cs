using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTCharacter : PlayerCharacter
{
    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        manaBase.GetComponent<SafeTMana>().SetCurrentAnger(1);

        base.TakeDamage(damage, defensePiercing);
    }
}
