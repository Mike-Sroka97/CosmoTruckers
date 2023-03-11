using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] List<CharacterSO> AllCharacters;
    [SyncVar] int playerNumber = 0;
    [SerializeField] [SyncVar] int PlayerID;
    CharacterSO Player;
    public CharacterSO GetPlayer { get => AllCharacters[PlayerID]; }

    [Command]
    public void CmdSetPlayerCharacter(int id)
    {
        PlayerID = id;
        RpcSetPlayer(id);
    }
    [ClientRpc]
    void RpcSetPlayer(int id)
    {
        foreach (var obj in AllCharacters)
            if (obj.PlayerID == PlayerID)
                Player = obj;
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this.gameObject);

        if (!hasAuthority)
        {
            StartCoroutine(SetPlayerName());
            return;
        }

        CmdSetPlayerNumber();
        gameObject.name = $"Player {NetworkTestManager.Instance.GetPlayerCount + 1}";
        NetworkTestManager.Instance.AddPlayers(this.gameObject);

    }
    public override void OnStopClient()
    {
        Destroy(this.gameObject);

        if (isServer) return;

        NetworkTestManager.Instance.RemovePlayer(this.gameObject);
        base.OnStopClient();
    }

    IEnumerator SetPlayerName()
    {
        yield return new WaitForSeconds(.5f);

        gameObject.name = $"Player {playerNumber}";
    }

    [Command(requiresAuthority = false)]
    void CmdSetPlayerNumber()
    {
        playerNumber = NetworkTestManager.Instance.GetPlayerCount + 1;
    }

    [Command]
    public void CmdGivePlayerItem(NetworkIdentity item)
    {
        item.AssignClientAuthority(connectionToClient);
    }

}
