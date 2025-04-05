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
    public bool EventFinished;
    public bool CombatDone;
    [SerializeField] private string[] dataFilesToUnlock;

    //Transforms for player to traverse per direction
    public List<Transform> SelectedTransforms = new List<Transform>();

    public int Row;

    public bool Active;

    protected SpriteRenderer myRenderer;
    private DungeonCharacter character;
    private DNode currentlySelectedNode;
    private LineRenderer currentLine;
    private DungeonController dungeon;
    int currentSelectedIndex = 0;
    List<Vector3> linePositions = new List<Vector3>();

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
        if (NodeData.GetComponent<DungeonCombatNode>() && !CombatDone)
        {
            StartCoroutine(NodeData.GetComponent<DungeonCombatNode>().StartCombat(this));
        }
        else if(NodeData.GetComponent<DungeonCombatNode>() && NodeData.GetComponent<DungeonCombatNode>().Boss && CombatDone)
        {
            StartCoroutine(CameraController.Instance.DungeonEnd(NodeData.GetComponent<DungeonCombatNode>().SceneToLoad));
        }
        else if(NodeData.GetComponent<DungeonEventNode>() && NodeData.GetComponent<DungeonEventNode>().Healing && !NodeData.GetComponent<DungeonEventNode>().Healed)
        {
            NodeData.GetComponent<DungeonEventNode>().Heal();
            NodeData.GetComponent<DungeonEventNode>().Healed = true;
            SetupLineRendererers();
        }
        else if (NodeData.GetComponent<DungeonEventNode>() && !EventFinished && !NodeData.GetComponent<DungeonEventNode>().Healing)
        {
            StartCoroutine(dungeon.NodeHandler.Move(true, this));
        }
        else if (Active)
        {
            CleanupLineRenderers();
            MoveToNode();
            return;
        }
    }

    private void MoveToNode()
    {
        Active = false;
        UnlockDataFiles();
        StartCoroutine(character.Move(currentlySelectedNode, linePositions));
        linePositions.Clear();
    }

    public void SelectNode(bool left)
    {
        if (NodeData.GetComponent<DungeonEventNode>() && !EventFinished)
            return;

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
        linePositions.Clear();
        SetupLineRendererers();
    }

    public virtual void SetupLineRendererers()
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
            if(Group % 2 == 0)
            {
                currentLine.SetPosition(i, new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y - 0.45f, nodePoint.transform.position.z));
                linePositions.Add(new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y, nodePoint.transform.position.z));
            }
            else
            {
                if(SelectableNodes[currentSelectedIndex].Group % 2 == 0 && nodePoint == points[points.Length - 1])
                {
                    currentLine.SetPosition(i, new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y -0.45f, nodePoint.transform.position.z));
                    linePositions.Add(new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y, nodePoint.transform.position.z));
                }
                else
                {
                    currentLine.SetPosition(i, new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y + 0.45f, nodePoint.transform.position.z));
                    linePositions.Add(new Vector3(nodePoint.transform.position.x, nodePoint.transform.position.y + 0.9f, nodePoint.transform.position.z));
                }

            }

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

         if (((!NodeData.GetComponent<DungeonCombatNode>()) && !NodeData.GetComponent<DungeonEventNode>()) || CombatDone)
            SetupLineRendererers();
    }

    private void UnlockDataFiles()
    {
        DataLogData dataLogData = SaveManager.LoadDataLogData();

        foreach(string dataFile in dataFilesToUnlock)
            dataLogData.SaveDataFileUnlock(dataFile);
    }
}
