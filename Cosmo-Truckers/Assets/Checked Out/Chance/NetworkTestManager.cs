using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkTestManager : NetworkBehaviour
{
    public static NetworkTestManager Instance;

    [SerializeField] [SyncVar] List<GameObject> Players = new List<GameObject>();
    [SerializeField] [SyncVar] int playerCount = 0;
    [SyncVar] int prevPlayerCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddPlayers(GameObject obj)
    {
        CmdAddPlayers(obj);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(GameObject obj)
    {
        Players.Add(obj);
        playerCount++;
    }

    private void Update()
    {
        if (!isServer) return;

        if(playerCount != prevPlayerCount)
        {
            foreach(GameObject player in Players)
            {
                player.GetComponent<TestNetworkColor>().ColorChange();
            }

            prevPlayerCount = playerCount;
        }
    }
}
