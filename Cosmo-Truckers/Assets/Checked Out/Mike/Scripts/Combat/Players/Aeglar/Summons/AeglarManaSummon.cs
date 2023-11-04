using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarManaSummon : EnemySummon
{
    [Header("0 = Veggie; 1 = Meat; 2 = Sweet")]
    [SerializeField] int manaType;

    const int manaToGive = 1;

    public override void Die()
    {
        FindObjectOfType<AeglarMana>().AdjustMana(manaToGive, manaType);
        base.Die();
    }
}
