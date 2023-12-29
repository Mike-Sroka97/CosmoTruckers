using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonNode : MonoBehaviour
{
    Node.DungeonNodeBase Node;

    public int GetConnections { get => Node.Connections; }

    public void SetNode(Node.DungeonNodeBase node)
    {
        Node = node;

        //Set Everything here
        GetComponent<Image>().sprite = node.NodeImage;
    }

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            OnDungeonClick();
        });
    }

    void OnDungeonClick()
    {
        CombatData.Instance.EnemysToSpawn = Node.EnemyHolder;

        foreach(var player in FindObjectsOfType<PlayerManager>())
        {
            CombatData.Instance.PlayersToSpawn.Add(player.GetPlayer.CombatPlayerSpawn);
        }

        if(NetworkManager.singleton)
        {
            NetworkManager.singleton.ServerChangeScene("Combat Test");
        }
        else
        {
            SceneManager.LoadScene("Combat Test");
        }
    }
}
