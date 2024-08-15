using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DNode : MonoBehaviour
{
    //Sprites for node
    [Header("Node Art")]
    [SerializeField] Sprite defaultNode;
    [SerializeField] Sprite combatNode;
    [SerializeField] Sprite bossNode;
    [SerializeField] Sprite badNode;
    [SerializeField] Sprite neutralNode;
    [SerializeField] Sprite goodNode;
    [SerializeField] Sprite healingNode;

    //Nodes that the player can move to
    [Space(20)]
    [Header("Node Assignment")]
    public OverworldNode UpNode;
    public OverworldNode LeftNode;
    public OverworldNode DownNode;
    public OverworldNode RightNode;

    //Interactivity of the node
    [Space(20)]
    [Header("Data")]
    [SerializeField] GameObject nodeData;

    //Transforms for player to traverse per direction
    public Transform[] UpTransforms;
    public Transform[] LeftTransforms;
    public Transform[] DownTransforms;
    public Transform[] RightTransforms;

    protected SpriteRenderer myRenderer;
    private bool chooseable;

    protected virtual void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();

        DetermineState();
    }

    public void DetermineState()
    {
        if(nodeData.GetComponent<DungeonCombatNode>())
        {
            DungeonCombatNode node = nodeData.GetComponent<DungeonCombatNode>();

            if (node.Boss)
                myRenderer.sprite = bossNode;
            else
                myRenderer.sprite = combatNode;
        }
        else if(nodeData.GetComponent<DungeonEventNode>())
        {
            DungeonEventNode node = nodeData.GetComponent<DungeonEventNode>();

            if (node.Bad)
                myRenderer.sprite = badNode;
            else if(node.Neutral)
                myRenderer.sprite = neutralNode;
            else if (node.Good)
                myRenderer.sprite = goodNode;
            else
                myRenderer.sprite = healingNode;
        }
        else
        {
            myRenderer.sprite = defaultNode;
        }
    }

    public void Interact()
    {
        if (nodeData.GetComponent<DungeonCombatNode>())
        {
            //TODO do something with the data
        }
        if(nodeData.GetComponent<DungeonEventNode>())
        {
            //TODO do something with the data
        }
    }

    public void ActivateNode()
    {
        chooseable = true;
    }

    public void DeactiveNode()
    {
        chooseable = false;
    }

    protected virtual void SetupLineRendererers()
    {
        LineRenderer currentLine;

        //Up line
        currentLine = transform.Find("LineRendererUp").GetComponent<LineRenderer>();

        if (UpNode && UpNode.Active)
        {
            if (UpNode.transform.position.x < transform.position.x)
                SetLinePositions(RightTransforms, currentLine, UpNode);
            else if (UpNode.transform.position.x > transform.position.x)
                SetLinePositions(LeftTransforms, currentLine, UpNode);
            else
                SetLinePositions(DownTransforms, currentLine, UpNode);
        }

        //Left line
        currentLine = transform.Find("LineRendererLeft").GetComponent<LineRenderer>();

        if (LeftNode && LeftNode.Active)
        {
            if (LeftNode.transform.position.y < transform.position.y)
                SetLinePositions(UpTransforms, currentLine, LeftNode);
            else if (LeftNode.transform.position.y > transform.position.y)
                SetLinePositions(DownTransforms, currentLine, LeftNode);
            else
                SetLinePositions(RightTransforms, currentLine, LeftNode);
        }

        //Down line
        currentLine = transform.Find("LineRendererDown").GetComponent<LineRenderer>();

        if (DownNode && DownNode.Active)
        {
            if (DownNode.transform.position.x < transform.position.x)
                SetLinePositions(RightTransforms, currentLine, DownNode);
            else if (DownNode.transform.position.x > transform.position.x)
                SetLinePositions(LeftTransforms, currentLine, DownNode);
            else
                SetLinePositions(UpTransforms, currentLine, DownNode);
        }

        //Right line
        currentLine = transform.Find("LineRendererRight").GetComponent<LineRenderer>();

        if (RightNode && RightNode.Active)
        {
            if (RightNode.transform.position.y < transform.position.y)
                SetLinePositions(DownTransforms, currentLine, RightNode);
            else if (RightNode.transform.position.y > transform.position.y)
                SetLinePositions(UpTransforms, currentLine, RightNode);
            else
                SetLinePositions(LeftTransforms, currentLine, RightNode);
        }
    }

    protected void SetLinePositions(Transform[] points, LineRenderer currentLine, OverworldNode node)
    {
        currentLine.positionCount = points.Length + 1;

        int i = 0;

        currentLine.SetPosition(i, node.transform.position);
        i++;

        foreach (Transform nodePoint in points)
        {
            currentLine.SetPosition(i, nodePoint.position);
            i++;
        }
    }

    private void CleanupLineRenderers()
    {
        LineRenderer currentLine;

        //Up line
        currentLine = transform.Find("LineRendererUp").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Left line
        currentLine = transform.Find("LineRendererLeft").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Down line
        currentLine = transform.Find("LineRendererDown").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Right line
        currentLine = transform.Find("LineRendererRight").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
    }

    //handles line renderers and active State
    public void SetupNode()
    {
        SetupLineRendererers();
    }

    public void LeavingNodeCleanup()
    {
        CleanupLineRenderers();
    }
}
