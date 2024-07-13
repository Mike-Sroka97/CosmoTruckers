using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldNode : MonoBehaviour
{
    //Nodes that the player can move to
    public OverworldNode UpNode;
    public OverworldNode LeftNode;
    public OverworldNode DownNode;
    public OverworldNode RightNode;

    //Interactivity of the node
    public bool Interactive = true;
    public bool Active = true;

    //Transforms for player to traverse per direction
    public Transform[] UpTransforms;
    public Transform[] LeftTransforms;
    public Transform[] DownTransforms;
    public Transform[] RightTransforms;

    private void Start()
    {
        if (!Active)
            GetComponent<SpriteRenderer>().color = Color.black;
        else if(!Interactive)
            GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
