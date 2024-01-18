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

        EnableButtonClick();
    }

    private void EnableButtonClick()
    {
        switch(Node.Type)
        {
            case EnumManager.NodeType.CombatNode:
                GetComponent<Button>().onClick.AddListener(delegate
                {
                    OnDungeonClick();
                });
                break;
            case EnumManager.NodeType.NCNode_SingleRandomPlayerAug:
                GetComponent<Button>().onClick.AddListener(delegate
                {
                    OnNCSingleRandomClick();
                });
                break;
            case EnumManager.NodeType.NCNode_PlayerOrderChoiceAug:
                GetComponent<Button>().onClick.AddListener(delegate
                {
                    OnNCAllCharacterClick();
                });
                break;
                break;
            case EnumManager.NodeType.RestNode:
                //TODO
                break;
            case EnumManager.NodeType.BossNode:
                //TODO
                break;

            default: GetComponent<Button>().interactable = false; break;
        }
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void OnDungeonClick()
    {
        CombatData.Instance.combatLocation = NodeLocation;
        CombatData.Instance.EnemysToSpawn = Node.EnemyHolder;
        CombatData.Instance.lastNode = lastNode;

        CombatData.Instance.PlayersToSpawn.Clear();

        foreach (var player in FindObjectsOfType<PlayerManager>())
        {
            CombatData.Instance.PlayersToSpawn.Add(player);
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

    void OnNCSingleRandomClick()
    {
        CombatData.Instance.combatLocation = NodeLocation;
        CombatData.Instance.lastNode = lastNode;

        //Get random player
        PlayerManager[] allPlayers = FindObjectsOfType<PlayerManager>();
        int index = Random.Range(0, allPlayers.Length - 1);
        DebuffStackSO stackToAdd = Instantiate(Node.AugToAdd[0]);
        bool added = false;

        foreach (DebuffStackSO aug in allPlayers[index].GetPlayerData.PlayerCurrentDebuffs)
        {
            if (string.Equals(aug.DebuffName, stackToAdd.DebuffName))
            {
                if (aug.Stackable && aug.CurrentStacks < aug.MaxStacks)
                {
                    aug.CurrentStacks += 1;
                    Debug.Log($"{allPlayers[index].GetPlayer.CharacterName} added stack of {Node.AugToAdd[0].DebuffName}");
                }
                else
                {
                    Debug.Log($"{allPlayers[index].GetPlayer.CharacterName} has max stacks of {Node.AugToAdd[0].DebuffName}");
                }

                added = true;
                break;
            }
        }

        if (!added)
        {
            allPlayers[index].GetPlayerData.PlayerCurrentDebuffs.Add(stackToAdd);
            Debug.Log($"{allPlayers[index].GetPlayer.CharacterName} has been given {Node.AugToAdd[0].DebuffName}");
        }

        SaveManager.Save(allPlayers[index].GetPlayerData, allPlayers[index].GetPlayer.PlayerID);

        GetComponent<Button>().interactable = false;

        StartCoroutine(RedrawMapDelay());
    }

    void OnNCAllCharacterClick()
    {
        CombatData.Instance.combatLocation = NodeLocation;
        CombatData.Instance.lastNode = lastNode;

        GameObject page = Instantiate(Node.EnemyHolder);
        page.GetComponent<DungeonNodeAllPlayerAUG>().SetUpPlayerOptions(Node.AugToAdd);

        StartCoroutine(RedrawMapDelay());
    }

    IEnumerator RedrawMapDelay()
    {
        yield return new WaitForSeconds(.5f);

        FindObjectOfType<DungeonGen>().RegenMap();
    }
}
