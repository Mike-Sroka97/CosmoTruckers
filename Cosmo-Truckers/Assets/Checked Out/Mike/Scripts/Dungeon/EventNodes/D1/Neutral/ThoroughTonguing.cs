using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoroughTonguing : EventNodeBase
{
    public void GetTongued()
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(999, 0);
        AddAugmentToPlayer(augmentsToAdd[0], 3);
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }
}
