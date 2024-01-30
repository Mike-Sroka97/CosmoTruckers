using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{
    public class DungeonNodeBase : MonoBehaviour
    {

        [Header("Node typing")]
        //What kind of node type this node is
        public EnumManager.NodeType Type;
        //If NC node what value is this node
        public EnumManager.NCNodeValue NCNodeValue;
        //How often this NC node is selected
        public int Weight;


        [Space(10)]
        //How many connections from this node
        public int Connections;
        //Display of the node
        public Sprite NodeImage;

        [Header("Exclude From")]
        //What dungeon to exclude this node from
        //If boss node what dungeon to spawn this boss in (trying to reuse variables for space)
        //-1 for testing nodes will not be excluded from any dungeon
        public int ExcludeFromDungeon = -1;

        [Header("Combat Nodes")]
        //Enemys to spawn in combat
        public GameObject EnemyHolder;

        [Header("NC Nodes")]
        public DebuffStackSO[] AugToAdd;
    }
}
