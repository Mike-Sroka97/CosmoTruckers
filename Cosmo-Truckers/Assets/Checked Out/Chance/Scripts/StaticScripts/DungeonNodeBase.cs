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
        //How many connections from this node
        public int Connections;
        //Name of the scene
        public string SceneName;
        //Display of the node
        public Sprite NodeImage;
        //Enemys to spawn in combat
        public GameObject EnemyHolder;
    }
}
