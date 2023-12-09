using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoCharacter : PlayerCharacter
{
    public override void StartTurn()
    {
        base.StartTurn();

        ProtoMana mana = manaBase.GetComponent<ProtoMana>();
        if (mana.CurrentRetention > 0 && !mana.ResetRetention)
            mana.ResetRetention = true;
        else if (mana.CurrentRetention > 0 && mana.ResetRetention)
            mana.ClearRetention();
    }
}
