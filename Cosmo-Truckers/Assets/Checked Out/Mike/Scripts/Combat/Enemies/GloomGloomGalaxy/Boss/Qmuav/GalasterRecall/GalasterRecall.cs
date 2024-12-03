using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalasterRecall : CombatMove
{
    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        FindObjectOfType<GalasterHordeAI>().Resurrect(100, true);
    }
}
