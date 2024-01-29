using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{
    public class DungeonNodeBase : MonoBehaviour
    {
        //How often this node is selected
        [System.Obsolete("If node weight help with keeping dungeons 'fair' can be re added back in")]
        public int Weight;

        [Header("Node typing")]
        //What kind of node type this node is
        public EnumManager.NodeType Type;

        //If NC node what value is this node
        public EnumManager.NCNodeValue NCNodeValue;


        [Space(10)]
        //How many connections from this node
        public int Connections;
        //Display of the node
        public Sprite NodeImage;

        [Header("Exclude From")]
        //What dungeon to exclude this node from
        //If boss node what dungeon to spawn this boss in (trying to reuse variables for space)
        public int ExcludeFromDungeon = -1;

        [Header("Combat Nodes")]
        //Enemys to spawn in combat
        public GameObject EnemyHolder;

        [Header("NC Nodes")]
        public DebuffStackSO[] AugToAdd;
    }
}
