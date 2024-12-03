using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstorBlaster : CombatMove
{
    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[0], baseDamage);
    }
}
