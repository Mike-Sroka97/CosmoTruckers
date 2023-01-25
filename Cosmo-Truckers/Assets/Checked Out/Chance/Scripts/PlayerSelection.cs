using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerSelection : NetworkBehaviour
{
    [SerializeField] GameObject[] PlayerSelections;
    [SerializeField] GameObject PlayerSelectionPreFab;

    [SerializeField] List<GameObject> ToShow = new List<GameObject>();

    void Awake()
    {
        NetworkTestManager.OnClientChange.AddListener(() => CmdAddNewPlayer());
    }
    private void OnDisable()
    {
        NetworkTestManager.OnClientChange.RemoveListener(() => CmdAddNewPlayer());
    }



    [Command(requiresAuthority = false)]
    void CmdAddNewPlayer()
    {
        for(int i = 0; i < ToShow.Count; i++)
        {
            if (ToShow[i] == null)
                ToShow.RemoveAt(i);
        }

        GameObject obj = Instantiate(
            PlayerSelectionPreFab
            );

        ToShow.Add(obj);

        NetworkServer.Spawn(
            obj, 
            NetworkTestManager.Instance.GetPlayers
            [NetworkTestManager.Instance.GetPlayers.Count - 1].
            GetComponent<NetworkIdentity>().connectionToClient
            );

        for (int i = 0; i < ToShow.Count; i++)
        {
            RpcShowAllActivePlayers(ToShow[i], i);
        }
    }

    [ClientRpc]
    void RpcShowAllActivePlayers(GameObject obj, int i)
    {
        obj.transform.SetParent(PlayerSelections[i].transform, false);
    }
}
