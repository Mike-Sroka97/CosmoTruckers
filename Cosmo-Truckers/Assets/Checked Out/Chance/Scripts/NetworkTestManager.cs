using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class NetworkTestManager : NetworkBehaviour
{
    public static NetworkTestManager Instance;
    public static UnityEvent OnClientChange = new UnityEvent();

    [SerializeField] [SyncVar] List<GameObject> Players = new List<GameObject>();
    public List<GameObject> GetPlayers { get => Players; }

    [SyncVar] int playerCount = 0;
    public int GetPlayerCount { get => playerCount; }
    [SyncVar] int prevPlayerCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddPlayers(GameObject obj)
    {
        CmdAddPlayers(obj);
    }
    public void RemovePlayer(GameObject obj)
    {
        if (NetworkClient.active)
            CmdRemovePlayer(obj);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(GameObject obj)
    {
        Players.Add(obj);
        playerCount++;
    }

    [Command(requiresAuthority = false)]
    public void CmdRemovePlayer(GameObject obj)
    {
        Players.Remove(obj);
        playerCount--;
    }

    private void Update()
    {
        //Only Host player may call this
        if (!isServer) return;

        //When ever we loose or gain a player
        if(playerCount != prevPlayerCount)
        {
            if(playerCount > prevPlayerCount)
                OnClientChange?.Invoke();

            prevPlayerCount = playerCount;
        }
    }

}
