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
    public List<DNode> SelectableNodes = new List<DNode>();

    //Interactivity of the node
    [Space(20)]
    [Header("Data")]
    public GameObject NodeData;
    public List<Transform> AdderTransforms;
    public int Group;
    public bool StartNode;
    public bool EndNode;

    //Transforms for player to traverse per direction
    public List<Transform> SelectedTransforms = new List<Transform>();

    public bool Active;

    protected SpriteRenderer myRenderer;
    private DungeonCharacter character;
    private DNode currentlySelectedNode;
    private LineRenderer currentLine;
    private DungeonController dungeon;
    int currentSelectedIndex = 0;

    protected virtual void Start()
    {
        DetermineState();
        character = FindObjectOfType<DungeonCharacter>();
        currentLine = transform.Find("LineRenderer").GetComponent<LineRenderer>();
        dungeon = FindObjectOfType<DungeonController>();
    }

    public void DetermineState()
    {
        myRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        if (NodeData.GetComponent<DungeonCombatNode>())
        {
            DungeonCombatNode node = NodeData.GetComponent<DungeonCombatNode>();

            if (node.Boss)
                myRenderer.sprite = bossNode;
            else
                myRenderer.sprite = combatNode;
        }
        else if(NodeData.GetComponent<DungeonEventNode>())
        {
            DungeonEventNode node = NodeData.GetComponent<DungeonEventNode>();

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
        if(Active)
        {
            CleanupLineRenderers();
            MoveToNode();
            return;
        }

        if (NodeData.GetComponent<DungeonCombatNode>())
        {
            //TODO do something with the data
        }
        if(NodeData.GetComponent<DungeonEventNode>())
        {
            //TODO do something with the data
        }
    }

    private void MoveToNode()
    {
        Active = false;
        StartCoroutine(character.Move(currentlySelectedNode, currentlySelectedNode.SelectedTransforms.ToArray()));
    }
    public void SelectNode(bool left)
    {
        if(left)
        {
            if(currentSelectedIndex - 1 >= 0)
                currentSelectedIndex--;
            else
                currentSelectedIndex = SelectableNodes.Count - 1;
        }
        else
        {
            if (currentSelectedIndex + 1 < SelectableNodes.Count)
                currentSelectedIndex++;
            else
                currentSelectedIndex = 0;
        }

        currentlySelectedNode = SelectableNodes[currentSelectedIndex];
        SetupLineRendererers();
    }

    protected virtual void SetupLineRendererers()
    {
        currentLine = transform.Find("LineRenderer").GetComponent<LineRenderer>();

        if (!currentlySelectedNode && SelectableNodes.Count > 0)
            currentlySelectedNode = SelectableNodes[currentSelectedIndex];

        if (currentlySelectedNode)
         SetLinePositions(currentlySelectedNode.SelectedTransforms.ToArray(), currentLine);
    }

    protected void SetLinePositions(Transform[] points, LineRenderer currentLine)
    {
        currentLine.positionCount = points.Length;

        int i = 0;

        foreach (Transform nodePoint in points)
        {
            currentLine.SetPosition(i, new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y - 0.45f, nodePoint.transform.position.z));
            i++;
        }
    }

    private void CleanupLineRenderers()
    {
        currentLine.positionCount = 0;
    }

    //handles line renderers and active State
    public void SetupNode()
    {
        if (AdderTransforms.Count > 0)
            foreach (DNode node in SelectableNodes)
                node.SelectedTransforms.InsertRange(0, AdderTransforms);

        if(EndNode)
        {
            DNode currentCombatNode;

            if (dungeon.transform.Find($"Constant Nodes/Combat ({Group + 2})"))
                currentCombatNode = dungeon.transform.Find($"Constant Nodes/Combat ({Group + 2})").GetComponent<DNode>();
            else
                currentCombatNode = dungeon.transform.Find("Constant Nodes/Rest Node").GetComponent<DNode>();

            currentCombatNode.SelectedTransforms.InsertRange(0, AdderTransforms);
            SelectableNodes.Add(currentCombatNode);
        }

        SetupLineRendererers();
    }

    public void LeavingNodeCleanup()
    {
        CleanupLineRenderers();
    }
}
