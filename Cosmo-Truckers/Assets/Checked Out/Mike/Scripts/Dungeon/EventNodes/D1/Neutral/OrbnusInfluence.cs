using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbnusInfluence : EventNodeBase
{
    public void EmbraceTheTitanicPressure()
    {
        foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
            player.MyCharacter.AddAugmentStack(augmentsToAdd[0]);

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
