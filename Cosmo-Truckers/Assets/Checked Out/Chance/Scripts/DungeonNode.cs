using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonNode : MonoBehaviour
{
    Node.DungeonNodeBase Node;
    public List<int> connections = new();
    [SerializeField] Vector2 NodeLocation = Vector2.zero;
    bool lastNode;

    public int GetConnections { get => Node.Connections; }
    public Vector2 GetNodeLocation { get => NodeLocation; }

    public void SetNode(Node.DungeonNodeBase node, Vector2 loc, bool last)
    {
        Node = node;
        lastNode = last;

        //Set Everything here
        NodeLocation = loc;
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
        CombatData.Instance.combatLocation = NodeLocation;
        CombatData.Instance.EnemysToSpawn = Node.EnemyHolder;
        CombatData.Instance.lastNode = lastNode;

        CombatData.Instance.PlayersToSpawn.Clear();

        foreach (var player in FindObjectsOfType<PlayerManager>())
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
