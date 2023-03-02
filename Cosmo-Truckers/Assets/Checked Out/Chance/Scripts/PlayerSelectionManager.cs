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
            if (obj.GetComponent<NetworkIdentity>().hasAuthority)
            {
                obj.GetComponent<PlayerSelection>().CmdReadyUp();
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
            StartCoroutine(SlowSpawnAI());
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
            //if (obj.GetComponent<NetworkIdentity>().hasAuthority)
            //{
            //    obj.GetComponent<PlayerSelection>().CmdReadyUp();
            //    ReadyButton.GetComponent<Image>().color = Color.green;
            //}

            AllReady += obj.GetComponent<PlayerSelection>().GetReady ? 1 : 0;

            print(AllReady + " : " + NetworkTestManager.Instance.GetPlayerCount);
        }

        return AllReady;
    }

    IEnumerator SlowSpawnAI()
    {
        for (int i = NetworkTestManager.Instance.GetPlayerCount; i < PlayerSelections.Length; i++)
        {
            GameObject obj = Instantiate(
                PlayerSelectionPreFab
            );

            ToShow.Add(obj);

            NetworkServer.Spawn(
                obj,
                NetworkTestManager.Instance.GetPlayers
                [0].
                GetComponent<NetworkIdentity>().connectionToClient
                );

            
            RpcShowAllActivePlayers(ToShow[i], i);

            yield return new WaitForEndOfFrame();

            obj.GetComponent<PlayerSelection>().CmdReadyUp();
        }

        //ReadyButton.GetComponent<Image>().color = Color.red;
        //GoButton.GetComponent<Button>().interactable = false;
        //GoButton.GetComponent<Image>().color = Color.red;
        GoButton.GetComponentInChildren<TMP_Text>().text = "Play";
    }

    [Command(requiresAuthority = false)]
    void CmdCheckIfReady()
    {
        int AllReady = ReadyCount();

        if (AllReady >= NetworkTestManager.Instance.GetPlayerCount)
        {
            GoButton.GetComponent<Button>().interactable = true;

            if (AllReady == NetworkManager.singleton.maxConnections)
                GoButton.GetComponentInChildren<TMP_Text>().text = "Play";

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

        //Turn off the play button
        //Obviosly they have no readied up yet
        CmdCheckIfReady();
    }

    [ClientRpc]
    void RpcShowAllActivePlayers(GameObject obj, int i)
    {
        obj.transform.SetParent(PlayerSelections[i].transform, false);
    }
}
