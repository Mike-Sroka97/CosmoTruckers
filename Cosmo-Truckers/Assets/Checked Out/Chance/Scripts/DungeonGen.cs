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

    public void GenerateMap()
    {
        int NodesToAdd = 1;
        int NodesToAddNext = 0;

        for (int i = 0; i < Levels.Length; i++)
        {
            List<int> weights = new List<int>();
            List<Node.DungeonNode> tempNodes = new List<Node.DungeonNode>(AllNodes);

            Levels[i].GetComponent<HorizontalLayoutGroup>().enabled = true;
            Levels[i].GetComponent<HorizontalLayoutGroup>().childAlignment = (TextAnchor)Random.Range(0, 9);

            while (NodesToAdd > 0)
            {
                //Add node weights to new list
                for (int j = 0; j < tempNodes.Count; j++)
                    weights.Add(tempNodes[j].Weight);


                int choice = MathCC.GetRandomWeightedIndex(weights);
                GameObject newNode = Instantiate(DungeonNodePreFab, Levels[i].transform);
                newNode.GetComponent<DungeonNode>().SetNode(tempNodes[choice]);

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
    }

    public void ShowConnections()
    {
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
    }

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
            myScript.GenerateMap();
        }

        if (GUILayout.Button("Show Connections"))
        {
            myScript.ShowConnections();
        }

        if (GUILayout.Button("Clear layout"))
        {
            myScript.ClearOldMap();
        }
    }
}
#endif

