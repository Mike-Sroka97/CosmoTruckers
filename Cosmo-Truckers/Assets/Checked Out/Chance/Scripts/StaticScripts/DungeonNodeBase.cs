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
        //What kind of node type this node is
        public EnumManager.NodeType Type;


        //How many connections from this node
        public int Connections;
        //Display of the node
        public Sprite NodeImage;

        [Header("Combat Nodes")]
        //Enemys to spawn in combat
        public GameObject EnemyHolder;

        [Header("NC Nodes")]
        public DebuffStackSO[] AugToAdd;
    }
}
