using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplicatingWormHole : EventNodeBase
{
    public void AcceptParticles()
    {
        foreach(PlayerVessel playerVessel in PlayerVesselManager.Instance.PlayerVessels)
            playerVessel.MyCharacter.DoubleAugment(1);

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
