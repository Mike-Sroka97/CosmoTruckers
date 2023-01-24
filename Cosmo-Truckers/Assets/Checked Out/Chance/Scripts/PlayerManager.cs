using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public override void OnStartClient()
    {
        if (!hasAuthority)
        {
            gameObject.name = $"Player{NetworkTestManager.Instance.GetPlayerCount + 1}";
            return;
        }

        NetworkTestManager.Instance.AddPlayers(this.gameObject);
    }
    public override void OnStopClient()
    {
        if (isServer) return;

        NetworkTestManager.Instance.RemovePlayer(this.gameObject);
        base.OnStopClient();
    }
}
