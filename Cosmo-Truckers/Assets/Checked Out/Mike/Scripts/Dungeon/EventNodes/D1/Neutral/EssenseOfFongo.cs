using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenseOfFongo : EventNodeBase
{
    public void AcceptDemofongo()
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RandomizeStats(20, 2, 2);
        IgnoreOption();
    }
}
