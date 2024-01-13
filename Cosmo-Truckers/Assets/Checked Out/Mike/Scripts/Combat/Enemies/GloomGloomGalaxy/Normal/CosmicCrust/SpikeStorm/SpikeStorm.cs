using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeStorm : CombatMove
{
    public override void EndMove()
    {
        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[0], baseDamage); //Player
        DealDamageOrHealing(CombatManager.Instance.CharactersSelected[1], 999); //Cosmic Crust
    }
}
