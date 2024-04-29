using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerSelectionManager : NetworkBehaviour
{
    [SerializeField] string HUBSceneName;
    [SerializeField] string TutorialSceneName;
    [SerializeField] GameObject[] PlayerSelections;
    [SerializeField] GameObject PlayerSelectionPreFab;

    [SerializeField] GameObject ReadyButton, GoButton;
    [SerializeField] List<GameObject> ToShow = new List<GameObject>();

    void Awake()
    {
        NetworkTestManager.OnClientChange.AddListener(() => CmdAddNewPlayer());
    }
    private void OnDisable()
    {
        NetworkTestManager.OnClientChange.RemoveListener(() => CmdAddNewPlayer());
    }

    public void ReadyUp()
    {
        foreach(var obj in GameObject.FindGameObjectsWithTag("PlayerSelection"))
        {
            if (obj.GetComponent<NetworkIdentity>().isOwned)
            {
                obj.GetComponent<PlayerSelection>().ReadyUp();
                ReadyButton.GetComponent<Image>().color = Color.green;
            }
        }

        CmdCheckIfReady();
    }

    public void AndGo()
    {
        CmdStartGame();
    }

    [Command(requiresAuthority = false)]
    void CmdStartGame()
    {
        NetworkManager.singleton.maxConnections = NetworkManager.singleton.numPlayers;

        if(ReadyCount() < 4)
        {
            //StartCoroutine(SlowSpawnAI());
        }
        else
        {
            //if(SaveData.newGame)
            //  NetworkManager.singleton.ServerChangeScene(TutorialSceneName);
            //else
            NetworkManager.singleton.ServerChangeScene(HUBSceneName);
        }
    }

    int ReadyCount()
    {
        int AllReady = 0;

        foreach (var obj in GameObject.FindGameObjectsWithTag("PlayerSelection"))
        {
            AllReady += obj.GetComponent<PlayerSelection>().GetReady ? 1 : 0;

            print(AllReady + " : " + NetworkTestManager.Instance.GetPlayerCount);
        }

        return AllReady;
    }

    [Command(requiresAuthority = false)]
    void CmdCheckIfReady()
    {
        int AllReady = ReadyCount();

        if (AllReady == 4)
        {
            GoButton.GetComponent<Button>().interactable = true;

            GoButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            GoButton.GetComponent<Button>().interactable = false;
            GoButton.GetComponent<Image>().color = Color.red;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdAddNewPlayer()
    {
        for(int i = 0; i < ToShow.Count; i++)
        {
            if (ToShow[i] == null)
                ToShow.RemoveAt(i);
        }

        if (NetworkTestManager.Instance.GetPlayers.Count == 1)
            GoButton.SetActive(true);

        for (int i = 0; i < ToShow.Count; i++)
        {
            RpcShowAllActivePlayers(ToShow[i], i);
        }

        //Turn off the play button
        //Obviosly they have not readied up yet
        CmdCheckIfReady();
    }

    public void AddCharacter()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;

        CmdAddCharacter(networkIdentity);
    }

    [Command(requiresAuthority = false)]
    void CmdAddCharacter(NetworkIdentity networkIdentity)
    {
        GameObject obj = Instantiate(
            PlayerSelectionPreFab
            );

        ToShow.Add(obj);

        NetworkServer.Spawn(
            obj,
            networkIdentity.connectionToClient
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
