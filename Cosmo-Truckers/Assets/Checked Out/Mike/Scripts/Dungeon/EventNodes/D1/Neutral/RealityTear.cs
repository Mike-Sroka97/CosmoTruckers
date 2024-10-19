using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityTear : EventNodeBase
{
    public void SwapAll()
    {
        //swap positions
        PlayerVessel[] players = PlayerVesselManager.Instance.PlayerVessels;
        MathHelpers.Shuffle(players);

        for (int i = 0; i < players.Length; i++)
            players[i].MyCharacter.FlipCharacter(i, true);

        StartCoroutine(SelectionChosen());
        IteratePlayerReference();
    }
}
