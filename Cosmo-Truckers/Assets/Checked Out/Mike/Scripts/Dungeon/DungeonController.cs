using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class DungeonController : MonoBehaviour
{
    [Header("Escape wheel")]
    [SerializeField] protected GameObject escapeWheel;
    [SerializeField] protected GameObject escapeMan;
    [SerializeField] protected float shakeSpeed = 2.0f;
    protected Vector2 shakeStart;

    protected Vector3 targetPos = Vector3.zero;

    [SerializeField] protected bool debugging;
    [SerializeField] protected GameObject[] nonCombatNodes;
    [SerializeField] int totalEventNodes = 24;
    [SerializeField] GameObject[] nodeLayouts;
    [SerializeField] protected Color nodeLayoutsColor; 
    [SerializeField] float timeToEscapeDungeon = 2f;
    [SerializeField] string sceneToLoad;
    public EventNodeHandler NodeHandler;
    public List<DNode> CombatNodes = new List<DNode>();

    public Transform PlayerStartPosition;
    public DNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;
    public float minCameraY;
    public float maxCameraY;
    public Transform CombatCameraPosition;

    protected bool enableLeaderOnFade = true;
    protected List<GameObject> negativeNodes;
    protected List<GameObject> neutralNodes;
    protected List<GameObject> positiveNodes;
    protected List<GameObject> determinedEventNodes;
    public int CurrentCombat = 0;

    private float currentTimeHeld = 0f;
    bool loading = false;

    private void Start()
    {
        //Get initial pos of escape wheel
        shakeStart.x = escapeWheel.transform.position.x;
        shakeStart.y = escapeWheel.transform.position.y;

        escapeWheel.gameObject.GetComponentInParent<Canvas>().worldCamera = Camera.main;

        NodeHandler = GetComponentInChildren<EventNodeHandler>();
        SetStartNode();
        DungeonInitialize();
        DetermineNodeTypes();
        DetermineNodeLayouts();
        SetupCombatNodes();
    }

    private void Update()
    {
        if (loading) return;

        if(Input.GetKey(KeyCode.Escape) && !CombatManager.Instance.InCombat)
        {
            currentTimeHeld += Time.deltaTime;

            //Set the fill amount for the radial
            escapeWheel.GetComponent<Image>().fillAmount = (currentTimeHeld / timeToEscapeDungeon) + .05f;

            //Set both image colors to become more red
            Color newColor = new Color(escapeWheel.GetComponent<Image>().color.r, escapeWheel.GetComponent<Image>().color.g - (Time.deltaTime / 2), escapeWheel.GetComponent<Image>().color.b - (Time.deltaTime / 2));
            escapeWheel.GetComponent<Image>().color = newColor;
            escapeMan.GetComponent<Image>().color = newColor;

            //Shake the wheel
            float shakeX = shakeStart.x;
            float shakeY = shakeStart.y;

            if (currentTimeHeld < .75f)
                shakeX = shakeStart.x + Mathf.Sin(currentTimeHeld * currentTimeHeld / timeToEscapeDungeon * shakeSpeed) * 0.1f;
            else
            {
                shakeX = shakeStart.x + Mathf.Sin(currentTimeHeld * currentTimeHeld / timeToEscapeDungeon * shakeSpeed) * 0.1f;
                shakeY = shakeStart.y + Mathf.Sin(currentTimeHeld * currentTimeHeld / timeToEscapeDungeon * shakeSpeed) * 0.1f;
            }

            escapeWheel.transform.position = new Vector3(shakeX, shakeY);
            escapeMan.transform.position = new Vector3(shakeX, shakeY);


            if (currentTimeHeld >= timeToEscapeDungeon)
            {
                loading = true;
                StartCoroutine(CameraController.Instance.DungeonEnd(sceneToLoad));
            }
        }
        else
        {
            currentTimeHeld -= Time.deltaTime;
            currentTimeHeld = Mathf.Clamp(currentTimeHeld, 0, timeToEscapeDungeon);

            //Set the fill amount for the radial
            escapeWheel.GetComponent<Image>().fillAmount = currentTimeHeld / timeToEscapeDungeon;

            if (currentTimeHeld > 0)
            {
                //Set both image colors to become less red
                Color newColor = new Color(escapeWheel.GetComponent<Image>().color.r, escapeWheel.GetComponent<Image>().color.g + (Time.deltaTime / 2), escapeWheel.GetComponent<Image>().color.b + (Time.deltaTime / 2));
                escapeWheel.GetComponent<Image>().color = newColor;
                escapeMan.GetComponent<Image>().color = newColor;

                escapeWheel.transform.position = shakeStart;
                escapeMan.transform.position = shakeStart;
            }
        }
    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade)
            CameraController.Instance.Leader.GetComponent<DungeonCharacter>().enabled = true;
    }

    private void SetStartNode()
    {
        if(!CurrentNode)
            CurrentNode = transform.Find("Constant Nodes/Start Node").GetComponent<DNode>();
    }

    protected abstract void DungeonInitialize();

    private void DetermineNodeTypes()
    {
        negativeNodes = new List<GameObject>();
        neutralNodes = new List<GameObject>();
        positiveNodes = new List<GameObject>();
        determinedEventNodes = new List<GameObject>();

        MathHelpers.Shuffle(nonCombatNodes);
        int currentNodeCount = 0;

        if(debugging)
        {
            for (int i = 0; i < nonCombatNodes.Length; i++)
            {
                neutralNodes.Add(nonCombatNodes[i]);
                currentNodeCount++;
                if (currentNodeCount >= totalEventNodes)
                    break;
            }
        }
        else
        {
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

        MathHelpers.Shuffle(nodeLayouts);

        List<DNode> allEventNodes = new List<DNode>();

        for(int i = 0; i < nodeLayoutPositions.Count; i++)
        {
            GameObject layoutInstance = Instantiate(nodeLayouts[i], nodeLayoutPositions[i]);

            // Set the color of every layout spawned here. The actual graphic is white
            layoutInstance.transform.Find("Node Path Graphic").GetComponent<SpriteRenderer>().color = nodeLayoutsColor;

            DNode[] nodes = layoutInstance.GetComponentsInChildren<DNode>();
            allEventNodes.AddRange(nodes);

            if (i % 2 != 0)
                foreach (DNode node in nodes)
                    node.transform.Find("Sprite").transform.Rotate(new Vector3(180, 0, 0));


            //determine combat node attachment stuffs
            foreach (DNode node in nodes)
            {
                // Set the color of every node's path spawned here. The actual graphic is white
                node.transform.Find("Event Node Path Sprite").GetComponent<SpriteRenderer>().color = nodeLayoutsColor;

                node.Group = i;

                if(node.StartNode)
                {
                    DNode currentCombatNode = transform.Find($"Constant Nodes/Combat ({i + 1})").GetComponent<DNode>();

                    node.SelectedTransforms.Insert(0, currentCombatNode.transform);
                    currentCombatNode.SelectableNodes.Add(node);
                }
            }
        }

        MathHelpers.Shuffle(determinedEventNodes);

        for(int i = 0; i < determinedEventNodes.Count; i++)
        {
            allEventNodes[i].NodeData = determinedEventNodes[i];
            allEventNodes[i].DetermineState();
            GameObject nodeArt = Instantiate(allEventNodes[i].NodeData, allEventNodes[i].transform);

            if(allEventNodes[i].Group % 2 != 0)
            {
                nodeArt.transform.localPosition -= new Vector3(0, 1f, 0);
                nodeArt.transform.Rotate(new Vector3(0, 0, 180));

                nodeArt.GetComponent<SpriteRenderer>().sortingOrder = 100 - allEventNodes[i].Row;
            }
            else
            {
                nodeArt.GetComponent<SpriteRenderer>().sortingOrder = allEventNodes[i].Row + 100;
            }
        }
    }

    private void SetupCombatNodes()
    {
        DNode[] nodes = GetComponentsInChildren<DNode>();

        int currentCombatNode = 0;

        foreach (DNode node in nodes)
            if (node.NodeData.GetComponent<DungeonCombatNode>())
            {
                CombatNodes.Add(node);
                CombatNodes[currentCombatNode].Group = currentCombatNode;
                currentCombatNode++;
            }
    }
}

