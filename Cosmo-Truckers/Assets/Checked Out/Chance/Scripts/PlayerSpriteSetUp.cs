using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpriteSetUp : NetworkBehaviour
{
    //TODO
    //This is really long and really ugly
    //Would like to come back and work on this at some point
    #region Show All Players Sprites
    public override void OnStartClient()
    {
        NetworkTestManager.OnClientChange.AddListener(delegate { StartCoroutine(ShowSprite()); });

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name == gameObject.name && player.GetComponent<PlayerManager>().authority)
            {
                    NetworkTestManager.Instance.CmdAddPlayers(gameObject);
            }
        }
    }
    IEnumerator ShowSprite()
    {
        yield return new WaitUntil(() => NetworkTestManager.Instance.GetPlayerCount == NetworkManager.singleton.maxConnections);

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.name == gameObject.name)
            {
                CmdShowClients(player.GetComponent<PlayerManager>());
            }
        }
    }
    [Command(requiresAuthority = false)]
    void CmdShowClients(PlayerManager pm)
    {
        RpcShowClients(pm);
    }
    [ClientRpc]
    void RpcShowClients(PlayerManager pm)
    {
        GetComponent<SpriteRenderer>().sprite = pm.GetComponent<PlayerManager>().GetPlayer.CharacterImage;
    }
    private void OnDisable()
    {
        NetworkTestManager.OnClientChange.RemoveListener(delegate { ShowSprite(); });
    }
    #endregion
}
