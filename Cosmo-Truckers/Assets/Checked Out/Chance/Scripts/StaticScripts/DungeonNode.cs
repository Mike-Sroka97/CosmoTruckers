using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Node
{

    [CreateAssetMenu(fileName = "DungeonNode", menuName = "ScriptableObjects/DungeonNode")]
    public class DungeonNode : ScriptableObject
    {
        //How often this node is selected
        public int Weight;
        //How many connections from this node
        public int Connections;
        //Name of the scene
        public string SceneName;
        //Display of the node
        public Sprite NodeImage;
    }
}
