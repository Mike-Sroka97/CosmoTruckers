using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGen : MonoBehaviour
{
    [Header("Node Data")]
    [SerializeField] GameObject[] Levels;
    [SerializeField] Node.DungeonNodeBase RestNode;
    [SerializeField] List<Node.DungeonNodeBase> CombatNodes;
    [SerializeField] List<Node.DungeonNodeBase> MiddleNodes;
    [SerializeField] List<Node.DungeonNodeBase> BossNode;
    [SerializeField] bool rest = true;
    [SerializeField] [Range(0, 2)] int spaceBetweenCombat = 2;

    [Header("Line Options")]
    [SerializeField] bool RandomAlignment = false;
    [SerializeField] Color LineStartColor;
    [SerializeField] Color LineEndColor;
    [SerializeField] float LineStartWidth;
    [SerializeField] float LineEndWidth;

    [Header("Prefabs")]
    [SerializeField] GameObject DungeonNodePreFab;
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject ConnectionHolder;

    [Header("Storage")]
    [SerializeField] [Tooltip("Set to 0 for unseeded")] int RandomSeed = 0;
    public int GetDungeonSeed { get => RandomSeed; }
    [SerializeField] List<DungeonData> CurrentLayout = new List<DungeonData>();
    #region Struct
    [System.Serializable]
    public struct DungeonData
    {
        public DungeonData(int level, List<Node.DungeonNodeBase> nodes, TextAnchor anchor, int seed)
        {
            Level = level;
            Nodes = nodes;
            AnchorPoint = anchor;

            Seed = seed;
        }
        public void Add(Node.DungeonNodeBase node)
        {
            Nodes.Add(node);
        }

        public int Level;
        public List<Node.DungeonNodeBase> Nodes;
        public TextAnchor AnchorPoint;

        public int Seed;
    }
    #endregion

    private void Start()
    {
        RegenMap();
    }

    public void RegenMap()
    {
        //Will set the seed if not first time in dungeon
        RandomSeed = CombatData.Instance.dungeonSeed;

        //For now just all combat nodes
        ClearOldMap();
        ClearMapMemory();
        GenerateMap();
        ShowMap();
    }

    /// <summary>
    /// Generate a new Dungeon in memory
    /// </summary>
    public void GenerateMap()
    {
        int rand = (int)System.DateTime.Now.Ticks;

        Random.InitState(RandomSeed != 0 ? RandomSeed : rand);
        if (RandomSeed == 0)
        {
            Debug.LogWarning($"Dungeon Seed: {rand}");
            CombatData.Instance.dungeonSeed = rand;
        }

        int NodesToAdd = 1;
        int NodesToAddNext = 0;

        List<Node.DungeonNodeBase> tempNodes = new List<Node.DungeonNodeBase>(MiddleNodes);

        for (int i = 0; i < Levels.Length; i++)
        {
            CurrentLayout.Add(new DungeonData(
                i,
                new List<Node.DungeonNodeBase>(),
                (TextAnchor)Random.Range(0, 9),
                RandomSeed != 0 ? RandomSeed : rand)
                );

            #region Based on weights (Obsolete)
            //List<int> weights = new List<int>();
            //while (NodesToAdd > 0)
            //{
            //    //Add node weights to new list
            //    for (int j = 0; j < tempNodes.Count; j++)
            //        weights.Add(tempNodes[j].Weight);


            //    int choice = MathCC.GetRandomWeightedIndex(weights);
            //    CurrentLayout[i].Add(tempNodes[choice]);

            //    if (tempNodes[choice].Connections > NodesToAddNext)
            //        NodesToAddNext = tempNodes[choice].Connections;

            //    tempNodes.RemoveAt(choice);
            //    weights.Clear();

            //    NodesToAdd--;
            //}
            #endregion
            #region Pure chaos mode
            //The Rest Node
            if (i == Levels.Length - 2 && rest)
            {
                CurrentLayout[i].Add(RestNode);
                NodesToAddNext = 1;
            }
            //The Boss Node
            else if (i == Levels.Length - 1)
            {
                CurrentLayout[i].Add(BossNode[Random.Range(0, BossNode.Count)]);
                NodesToAddNext = 0;
            }
            //Combat Nodes 
            //+1 for easy to enter on serialized feild
            else if (i % (spaceBetweenCombat + 1) == 0)
            {
                int choice = Random.Range(0, CombatNodes.Count);
                CurrentLayout[i].Add(CombatNodes[choice]);
                NodesToAddNext = CombatNodes[choice].Connections;
            }
            //Random Middle Nodes
            else
            {
                while (NodesToAdd > 0)
                {
                    //Temparary while there are not a ton of test nodes to keep from cycling tho them all
                    //TODO 
                    //Remove
                    if (tempNodes.Count == 0) tempNodes = new List<Node.DungeonNodeBase>(MiddleNodes);

                    int choice = Random.Range(0, tempNodes.Count);

                    CurrentLayout[i].Add(tempNodes[choice]);

                    if (tempNodes[choice].Connections > NodesToAddNext)
                        NodesToAddNext = tempNodes[choice].Connections;

                    tempNodes.RemoveAt(choice);

                    NodesToAdd--;
                }
            }
            #endregion

            NodesToAdd = NodesToAddNext;
            NodesToAddNext = 0;
        }

        tempNodes.Clear();

        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    /// <summary>
    /// Pull back up old Dungeon
    /// </summary>
    /// <param name="data">The previous dungeon data to load</param>
    /// <param name="showMap">Should the new map be instantly show on screen</param>
    public void LoadInMap(List<DungeonData> data, bool showMap = true)
    {
        ClearMapMemory();

        CurrentLayout = new List<DungeonData>(data);
        if (showMap)
            ShowMap();
    }

    /// <summary>
    /// Displays the map on screen
    /// </summary>
    public void ShowMap()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].GetComponent<VerticalLayoutGroup>().enabled = true;
            if (RandomAlignment)
                Levels[i].GetComponent<VerticalLayoutGroup>().childAlignment = CurrentLayout[i].AnchorPoint;

            for (int j = 0; j < CurrentLayout[i].Nodes.Count; j++)
            {
                GameObject newNode = Instantiate(DungeonNodePreFab, Levels[i].transform);
                newNode.GetComponent<DungeonNode>().SetNode(CurrentLayout[i].Nodes[j], new Vector2(i + 1, j), i == Levels.Length - 1 ? true : false);
            }
        }
        StartCoroutine(ShowConnections());
    }

    IEnumerator ShowConnections()
    {
        yield return new WaitForEndOfFrame();

        Random.InitState(RandomSeed != 0 ? RandomSeed : CurrentLayout[0].Seed);

        for (int i = 0; i < Levels.Length - 1; i++) //Total 'Levels' Minus boss level
        {
            for (int j = 0; j < Levels[i].transform.childCount; j++) //Nodes in level
            {
                List<int> connections = new List<int>();

                for (int k = 0; k < Levels[i].transform.GetChild(j).GetComponent<DungeonNode>().GetConnections; k++) //Connections for this node
                {
                    int Connection = j > Levels[i + 1].transform.childCount -1 ? Levels[i + 1].transform.childCount -1 : j;// Random.Range(0, Levels[i + 1].transform.childCount);

                    if (Levels[i + 1].transform.childCount > 1)
                    {
                        while (connections.Contains(Connection))
                            Connection = Random.Range(0, Levels[i + 1].transform.childCount);
                    }

                    connections.Add(Connection);
                    Levels[i].transform.GetChild(j).GetComponent<DungeonNode>().connections.Add(Connection);
                }

                List<int> connectionsToRemove = new List<int>();
                //Draw each line in connections
                foreach (var conn in connections)
                {
                    LineRenderer newLine = Instantiate(line, ConnectionHolder.transform);
                    newLine.startColor = LineStartColor;
                    newLine.endColor = LineEndColor;
                    newLine.endWidth = LineEndWidth;
                    newLine.startWidth = LineStartWidth;

                    newLine.SetPositions(new Vector3[] { Levels[i].transform.GetChild(j).transform.position + (Vector3.right / 4f), Levels[i + 1].transform.GetChild(conn).transform.position + (Vector3.left / 4f) });

                    if (j > 0)
                    {
                        foreach (var previousConns in Levels[i].transform.GetChild(j - 1).GetComponent<DungeonNode>().connections)
                        {
                            if (lineSegmentsIntersect(Levels[i].transform.GetChild(j).transform.position + (Vector3.right / 5f),
                                                      Levels[i + 1].transform.GetChild(conn).transform.position + (Vector3.left / 5f),
                                                      Levels[i].transform.GetChild(j - 1).transform.position + (Vector3.right / 5f),
                                                      Levels[i + 1].transform.GetChild(previousConns).transform.position + (Vector3.left / 5f)))
                            {
                                Debug.LogError($"Intersection removed from level {i + 1} node {j + 1} to level {i + 2} node {conn + 1}");
                                connectionsToRemove.Add(conn);
                                Destroy(newLine);
                            }
                        }
                    }
                }

                foreach(var rem in connectionsToRemove)
                {
                    Levels[i].transform.GetChild(j).GetComponent<DungeonNode>().connections.Remove(rem);
                }

                //Set what dungeon is activly selectable
                //Set the first node to active if first time in map
                if (CombatData.Instance.combatLocation == Vector2.zero)
                {
                    if (new Vector2(i, j) == Vector2.zero)
                        Levels[i].transform.GetChild(j).GetComponent<Button>().interactable = true;
                }
                else if (CombatData.Instance.combatLocation.x == i)
                {
                    //If active location but no connection
                    if (Levels[i - 1].transform.GetChild((int)CombatData.Instance.combatLocation.y).GetComponent<DungeonNode>().connections.Contains(j))
                        Levels[i].transform.GetChild(j).GetComponent<Button>().interactable = true;
                }
            }
        }

        //Set if boss node is active
        if (CombatData.Instance.combatLocation.x == Levels.Length - 1)
        {
            Levels[Levels.Length - 1].transform.GetChild(0).GetComponent<Button>().interactable = true;
        }

        Random.InitState((int)System.DateTime.Now.Ticks);
    }


    /// <summary>
    /// Removes all points and dungeon nodes from screen
    /// </summary>
    public void ClearOldMap()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            int count = Levels[i].transform.childCount;
            for (int j = 0; j < count; j++)
            {
                Destroy(Levels[i].transform.GetChild(j).gameObject);
            }
        }

        for (int i = 0; i < ConnectionHolder.transform.childCount; i++)
            Destroy(ConnectionHolder.transform.GetChild(i).gameObject);
    }
    /// <summary>
    /// Removes the old Dungeon layout from the loaded memory
    /// </summary>
    public void ClearMapMemory() => CurrentLayout.Clear();

    public static bool lineSegmentsIntersect(Vector2 lineOneStart, Vector2 lineOneEnd, Vector2 lineTwoStart, Vector2 lineTwoEnd) 
    {
        return 
            (((lineTwoEnd.y - lineOneStart.y) * (lineTwoStart.x - lineOneStart.x) > 
            (lineTwoStart.y - lineOneStart.y) * (lineTwoEnd.x - lineOneStart.x)) != 
            ((lineTwoEnd.y - lineOneEnd.y) * (lineTwoStart.x - lineOneEnd.x) > 
            (lineTwoStart.y - lineOneEnd.y) * (lineTwoEnd.x - lineOneEnd.x)) && 
            ((lineTwoStart.y - lineOneStart.y) * (lineOneEnd.x - lineOneStart.x) > 
            (lineOneEnd.y - lineOneStart.y) * (lineTwoStart.x - lineOneStart.x)) != 
            ((lineTwoEnd.y - lineOneStart.y) * (lineOneEnd.x - lineOneStart.x) > 
            (lineOneEnd.y - lineOneStart.y) * (lineTwoEnd.x - lineOneStart.x))); 
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonGen))]
public class DungeonGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonGen myScript = (DungeonGen)target;
        if (GUILayout.Button("Generate new layout"))
        {
            myScript.ClearOldMap();
            myScript.ClearMapMemory();
            myScript.GenerateMap();
            myScript.ShowMap();
        }

        if(GUILayout.Button("ReDraw Map"))
        {
            myScript.ShowMap();
        }

        if (GUILayout.Button("Clear Screen"))
        {
            myScript.ClearOldMap();
        }
    }
}
#endif

