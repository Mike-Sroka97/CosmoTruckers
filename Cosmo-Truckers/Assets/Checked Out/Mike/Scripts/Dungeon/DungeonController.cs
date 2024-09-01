using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DungeonController : MonoBehaviour
{
    [SerializeField] protected bool debugging;
    [SerializeField] protected GameObject[] nonCombatNodes;
    [SerializeField] protected GameObject[] combatNodes;
    [SerializeField] int totalEventNodes = 24;
    [SerializeField] GameObject[] nodeLayouts;

    public DNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;
    public float minCameraY;
    public float maxCameraY;

    protected bool enableLeaderOnFade = true;
    protected List<GameObject> negativeNodes;
    protected List<GameObject> neutralNodes;
    protected List<GameObject> positiveNodes;
    protected List<GameObject> determinedEventNodes;


    private void Start()
    {
        SetStartNode();
        DungeonInitialize();
        DetermineNodeTypes();
        DetermineNodeLayouts();
    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade)
            CameraController.Instance.Leader.GetComponent<DungeonCharacter>().enabled = true;
    }

    private void SetStartNode()
    {
        CurrentNode = transform.Find("Constant Nodes/Start Node").GetComponent<DNode>();
    }

    protected abstract void DungeonInitialize();

    private void DetermineNodeTypes()
    {
        negativeNodes = new List<GameObject>();
        neutralNodes = new List<GameObject>();
        positiveNodes = new List<GameObject>();
        determinedEventNodes = new List<GameObject>();

        MathCC.Shuffle(nonCombatNodes);
        int currentNodeCount = 0;

        for (int i = 0; i < nonCombatNodes.Length; i++)
        {
            if (nonCombatNodes[i].GetComponent<DungeonEventNode>().Good && positiveNodes.Count < totalEventNodes / 4)
            {
                positiveNodes.Add(nonCombatNodes[i]);
                currentNodeCount++;
            }
            else if (nonCombatNodes[i].GetComponent<DungeonEventNode>().Neutral && neutralNodes.Count < totalEventNodes / 2)
            {
                neutralNodes.Add(nonCombatNodes[i]);
                currentNodeCount++;

            }
            else if (nonCombatNodes[i].GetComponent<DungeonEventNode>().Bad && negativeNodes.Count < totalEventNodes / 4)
            {
                negativeNodes.Add(nonCombatNodes[i]);
                currentNodeCount++;
            }

            if (currentNodeCount >= totalEventNodes)
                break;
        }

        determinedEventNodes.AddRange(positiveNodes);
        determinedEventNodes.AddRange(neutralNodes);
        determinedEventNodes.AddRange(negativeNodes);
    }

    private void DetermineNodeLayouts()
    {
        List<Transform> nodeLayoutPositions = new List<Transform>();
        Transform[] nodeLayoutTransforms = transform.Find("NodeLayoutTransforms").GetComponentsInChildren<Transform>();

        for (int i = 0; i < nodeLayoutTransforms.Length; i++)
        {
            if (i == 0)
                continue;

            nodeLayoutPositions.Add(nodeLayoutTransforms[i]);
        }

        MathCC.Shuffle(nodeLayouts);

        List<DNode> allEventNodes = new List<DNode>();

        for(int i = 0; i < nodeLayoutPositions.Count; i++)
        {
            GameObject layoutInstance = Instantiate(nodeLayouts[i], nodeLayoutPositions[i]);

            DNode[] nodes = layoutInstance.GetComponentsInChildren<DNode>();
            allEventNodes.AddRange(nodes);

            if (i % 2 != 0)
                foreach (DNode node in nodes)
                    node.transform.Rotate(new Vector3(180, 0, 0));

            //determine combat node attachement stuffs
            foreach(DNode node in nodes)
            {
                node.Group = i;

                if(node.StartNode)
                {
                    DNode currentCombatNode = transform.Find($"Constant Nodes/Combat ({i + 1})").GetComponent<DNode>();

                    node.SelectedTransforms.Insert(0, currentCombatNode.transform);
                    currentCombatNode.SelectableNodes.Add(node);
                }
            }
        }

        MathCC.Shuffle(determinedEventNodes);

        for(int i = 0; i < determinedEventNodes.Count; i++)
        {
            allEventNodes[i].NodeData = determinedEventNodes[i];
            allEventNodes[i].DetermineState();
        }
    }
}

