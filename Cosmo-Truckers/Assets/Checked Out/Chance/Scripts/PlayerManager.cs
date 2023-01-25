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
            gameObject.name = $"Co-OpPlayer";
            return;
        }

        gameObject.name = $"Player{NetworkTestManager.Instance.GetPlayerCount + 1}";
        NetworkTestManager.Instance.AddPlayers(this.gameObject);
    }
    public override void OnStopClient()
    {
        if (isServer) return;

        NetworkTestManager.Instance.RemovePlayer(this.gameObject);
        base.OnStopClient();
    }

    [Command]
    public void CmdGivePlayerItem(NetworkIdentity item)
    {
        item.AssignClientAuthority(connectionToClient);
    }

}
