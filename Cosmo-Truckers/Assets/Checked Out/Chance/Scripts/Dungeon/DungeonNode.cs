//using Mirror;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class DungeonNode : MonoBehaviour
//{
//    Node.DungeonNodeBase Node;
//    public List<int> connections = new();
//    [SerializeField] Vector2 NodeLocation = Vector2.zero;
//    bool lastNode;

//    public int GetConnections { get => Node.Connections; }
//    public Vector2 GetNodeLocation { get => NodeLocation; }

//    /// <summary>
//    /// Called from dungeon gen to set the current nodes value
//    /// </summary>
//    /// <param name="node">The node data for this location</param>
//    /// <param name="loc">the current location of this node</param>
//    /// <param name="last">is this the last node in the dungeon</param>
//    public void SetNode(Node.DungeonNodeBase node, Vector2 loc, bool last)
//    {
//        Node = node;
//        lastNode = last;

//        //Set Everything here
//        NodeLocation = loc;
//        GetComponent<Image>().sprite = node.NodeImage;

//        EnableButtonClick();
//    }

//    /// <summary>
//    /// Sets what the nodes button does on click based on the node type
//    /// </summary>
//    private void EnableButtonClick()
//    {
//        switch(Node.Type)
//        {
//            //Combat and boss will both funcion the same
//            //Just take the player into combat
//            case EnumManager.NodeType.CombatNode:
//            case EnumManager.NodeType.BossNode:
//                GetComponent<Button>().onClick.AddListener(delegate
//                {
//                    OnDungeonClick();
//                });
//                break;
//            case EnumManager.NodeType.NCNode_SingleRandomPlayerAug:
//                GetComponent<Button>().onClick.AddListener(delegate
//                {
//                    OnNCSingleRandomClick();
//                });
//                break;
//            case EnumManager.NodeType.NCNode_PlayerOrderChoiceAug:
//            case EnumManager.NodeType.NCNode_PlayerDependent:
//            case EnumManager.NodeType.NCNode_PartyVoting:
//            case EnumManager.NodeType.NCNode_PartyDistribution:
//            case EnumManager.NodeType.NCNode_Auto:
//                GetComponent<Button>().onClick.AddListener(delegate
//                {
//                    OnNCAllCharacterClick();
//                });
//                break;
//            case EnumManager.NodeType.RestNode:
//                GetComponent<Button>().onClick.AddListener(delegate
//                {
//                    OnRestClick();
//                });
//                break;

//                //If the node type has not been set up print to console to enure it gets created
//            default: Debug.LogError($"{Node.Type} has not been set up"); GetComponent<Button>().interactable = false; break;
//        }
//    }

//    private void OnDisable()
//    {
//        GetComponent<Button>().onClick.RemoveAllListeners();
//    }

//    /// <summary>
//    /// Sets all combat data and sends player into predetermened battle
//    /// </summary>
//    void OnDungeonClick()
//    {
//        CombatData.Instance.combatLocation = NodeLocation;
//        CombatData.Instance.EnemysToSpawn = Node.EnemyHolder;
//        CombatData.Instance.lastNode = lastNode;


//        //Call to load in enemys
//        EnemyManager.Instance.InitializeEnemys();

//        //Close INA
//        FindObjectOfType<INAcombat>().CloseDungeonPage();
//    }

//    void OnNCSingleRandomClick()
//    {
//        CombatData.Instance.combatLocation = NodeLocation;
//        CombatData.Instance.lastNode = lastNode;

//        //Get random player
//        List<PlayerCharacter> allPlayers = EnemyManager.Instance.GetAlivePlayerCharacters();

//        int index = Random.Range(0, allPlayers.Count - 1);
//        DebuffStackSO stackToAdd = Instantiate(Node.AugToAdd[0]);
//        bool added = false;

//        foreach (DebuffStackSO aug in allPlayers[index].GetAUGS)
//        {
//            if (string.Equals(aug.DebuffName, stackToAdd.DebuffName))
//            {
//                if (aug.Stackable && aug.CurrentStacks < aug.MaxStacks)
//                {
//                    aug.CurrentStacks += 1;
//                    Debug.Log($"{allPlayers[index].CharacterName} added stack of {Node.AugToAdd[0].DebuffName}");
//                }
//                else
//                {
//                    Debug.Log($"{allPlayers[index].CharacterName} has max stacks of {Node.AugToAdd[0].DebuffName}");
//                }

//                added = true;
//                break;
//            }
//        }

//        if (!added)
//        {
//            allPlayers[index].AddDebuffStack(stackToAdd);
//            Debug.Log($"{allPlayers[index].CharacterName} has been given {Node.AugToAdd[0].DebuffName}");
//        }

//        GetComponent<Button>().interactable = false;

//        StartCoroutine(RedrawMapDelay());
//    }

//    void OnNCAllCharacterClick()
//    {
//        CombatData.Instance.combatLocation = NodeLocation;
//        CombatData.Instance.lastNode = lastNode;

//        if(!CombatData.Instance.skipNCNode)
//            Instantiate(Node.EnemyHolder, FindObjectOfType<DungeonGen>().GetBG.transform).GetComponent<NCNodePopUpOptions>().SetUp(Node.AugToAdd);
//        else
//            CombatData.Instance.skipNCNode = false;

//        StartCoroutine(RedrawMapDelay());
//    }

//    void OnRestClick()
//    {
//        //Set the next and last node
//        CombatData.Instance.combatLocation = NodeLocation;
//        CombatData.Instance.lastNode = lastNode;

//        //Heal all players
//        List<PlayerCharacter> allPlayers = EnemyManager.Instance.Players;

//        foreach (var player in allPlayers)
//        {
//            ////Set amount of health to add
//            //int healthToAdd = 50;
//            //int maxHealth = player.GetPlayer.CombatPlayerSpawn.GetComponent<PlayerCharacter>().Health;
//            //player.GetPlayerData.PlayerCurrentHP = player.GetPlayerData.PlayerCurrentHP + healthToAdd > maxHealth ? -1 : player.GetPlayerData.PlayerCurrentHP + healthToAdd;

//            //Give max health for now
//            player.CurrentHealth = player.Health;
//        }

//        //Redraw the map
//        StartCoroutine(RedrawMapDelay());
//    }

//    //Redraws the map with the players new location
//    IEnumerator RedrawMapDelay()
//    {
//        yield return new WaitForSeconds(.5f);

//        FindObjectOfType<DungeonGen>().RegenMap();
//    }
//}
