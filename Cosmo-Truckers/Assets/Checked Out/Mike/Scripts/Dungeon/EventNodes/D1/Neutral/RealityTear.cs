using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityTear : EventNodeBase
{
    public void SwapAll()
    {
        //swap positions
        int[] players = new int[] { 0, 1, 2, 3 };
        HelperFunctions.Shuffle(players);

        for (int i = 0; i < PlayerVesselManager.Instance.PlayerVessels.Length; i++)
            PlayerVesselManager.Instance.PlayerVessels[players[i]].MyCharacter.FlipCharacter(i, true);

        StartCoroutine(SelectionChosen());
        IteratePlayerReference();
    }
}
