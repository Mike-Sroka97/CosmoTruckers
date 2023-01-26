using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGen : MonoBehaviour
{
    [SerializeField] GameObject[] Levels;
    [SerializeField] List<Node.DungeonNode> AllNodes;

    [Header("Line Options")]
    [SerializeField] Color LineStartColor;
    [SerializeField] Color LineEndColor;
    [SerializeField] float LineStartWidth;
    [SerializeField] float LineEndWidth;

    [Header("Prefabs")]
    [SerializeField] GameObject DungeonNodePreFab;
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject ConnectionHolder;

    [Header("Storage")]
    [SerializeField][Tooltip("Set to 0 for unseeded")] int RandomSeed = 0;
    [SerializeField] int StartingNodes = 1;
    [SerializeField] List<DungeonData> CurrentLayout = new List<DungeonData>();
    #region Struct
    [System.Serializable]
    public struct DungeonData
    {
        public DungeonData(int level, List<Node.DungeonNode> nodes, TextAnchor anchor, int seed)
        {
            Level = level;
            Nodes = nodes;
            AnchorPoint = anchor;

            Seed = seed;
        }
        public void Add(Node.DungeonNode node)
        {
            Nodes.Add(node);
        }

        public int Level;
        public List<Node.DungeonNode> Nodes;
        public TextAnchor AnchorPoint;

        public int Seed;
    }
    #endregion

    /// <summary>
    /// Generate a new Dungeon in memory
    /// </summary>
    public void GenerateMap()
    {
        int rand = (int)System.DateTime.Now.Ticks;

        Random.InitState(RandomSeed != 0 ? RandomSeed : rand);
        if (RandomSeed == 0)
            Debug.LogWarning($"Dungeon Seed: {rand}");

        int NodesToAdd = StartingNodes;
        int NodesToAddNext = 0;

        for (int i = 0; i < Levels.Length; i++)
        {
            List<int> weights = new List<int>();
            List<Node.DungeonNode> tempNodes = new List<Node.DungeonNode>(AllNodes);

            CurrentLayout.Add(new DungeonData(
                i,
                new List<Node.DungeonNode>(),
                (TextAnchor)Random.Range(0, 9),
                RandomSeed != 0 ? RandomSeed : rand)
                );

            while (NodesToAdd > 0)
            {
                //Add node weights to new list
                for (int j = 0; j < tempNodes.Count; j++)
                    weights.Add(tempNodes[j].Weight);


                int choice = MathCC.GetRandomWeightedIndex(weights);
                CurrentLayout[i].Add(tempNodes[choice]);

                if (tempNodes[choice].Connections > NodesToAddNext)
                    NodesToAddNext = tempNodes[choice].Connections;

                tempNodes.RemoveAt(choice);
                weights.Clear();

                NodesToAdd--;
            }

            NodesToAdd = NodesToAddNext;
            NodesToAddNext = 0;


            tempNodes.Clear();
        }

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
            Levels[i].GetComponent<HorizontalLayoutGroup>().enabled = true;
            Levels[i].GetComponent<HorizontalLayoutGroup>().childAlignment = CurrentLayout[i].AnchorPoint;

            for(int j = 0; j < CurrentLayout[i].Nodes.Count; j++)
            {
                GameObject newNode = Instantiate(DungeonNodePreFab, Levels[i].transform);
                newNode.GetComponent<DungeonNode>().SetNode(CurrentLayout[i].Nodes[j]);
            }
        }
        StartCoroutine(ShowConnections());
    }
    IEnumerator ShowConnections()
    {
        yield return new WaitForEndOfFrame();

        Random.InitState(RandomSeed != 0 ? RandomSeed : CurrentLayout[0].Seed);

        for (int i = 0; i < Levels.Length; i++)
            Levels[i].GetComponent<HorizontalLayoutGroup>().enabled = false;

        for (int i = 0; i < Levels.Length - 1; i++)
        {
            for (int j = 0; j < Levels[i].transform.childCount; j++)
            {

                List<int> connections = new List<int>();

                for (int k = 0; k < Levels[i].transform.GetChild(j).GetComponent<DungeonNode>().GetConnections; k++)
                {
                    LineRenderer newLine = Instantiate(line, ConnectionHolder.transform);
                    newLine.startColor = LineStartColor;
                    newLine.endColor = LineEndColor;
                    newLine.endWidth = LineEndWidth;
                    newLine.startWidth = LineStartWidth;

                    int Connection = Random.Range(0, Levels[i + 1].transform.childCount);
                    while(connections.Contains(Connection))
                        Connection = Random.Range(0, Levels[i + 1].transform.childCount);

                    connections.Add(Connection);

                    newLine.SetPositions(new Vector3[] { Levels[i].transform.GetChild(j).transform.position, Levels[i + 1].transform.GetChild(Connection).transform.position });
                    //newLine.SetPosition(1, Levels[i + 1].transform.GetChild(Random.Range(0, Levels[i + 1].transform.childCount)).transform.position);
                }
            }
        }
        Random.InitState((int)System.DateTime.Now.Ticks);
    }


    /// <summary>
    /// Removes all points and dungeon nodes from screen
    /// </summary>
    public void ClearOldMap()
    {
        for(int i = 0; i < Levels.Length; i++)
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

