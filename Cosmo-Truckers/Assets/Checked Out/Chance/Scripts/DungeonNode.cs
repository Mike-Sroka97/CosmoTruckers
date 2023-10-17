using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Debug.LogError("TODO: What ever the on click will be");
        });
    }
}
