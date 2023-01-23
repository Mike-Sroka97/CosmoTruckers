using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonNode : MonoBehaviour
{
    Node.DungeonNode Node;

    public int GetConnections { get => Node.Connections; }

    public void SetNode(Node.DungeonNode node)
    {
        Node = node;

        //Set Everything here
        GetComponent<Image>().sprite = node.NodeImage;
    }
}
