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

    public OverworldNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;
    public float minCameraY;
    public float maxCameraY;

    protected bool enableLeaderOnFade = true;
    protected List<GameObject> negativeNodes;
    protected List<GameObject> neutralNodes;
    protected List<GameObject> positiveNodes;

    private void Start()
    {
        DungeonInitialize();
        DetermineNodeTypes();
    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade)
            CameraController.Instance.Leader.GetComponent<OverworldCharacter>().enabled = true;
    }

    protected abstract void DungeonInitialize();

    protected void DetermineNodeTypes()
    {
        negativeNodes = new List<GameObject>();
        neutralNodes = new List<GameObject>();
        positiveNodes = new List<GameObject>();

        MathCC.Shuffle(nonCombatNodes);
        int currentNodeCount = 0;

        for(int i = 0; i < nonCombatNodes.Length; i++)
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
            else if(nonCombatNodes[i].GetComponent<DungeonEventNode>().Bad && negativeNodes.Count < totalEventNodes / 4)
            {
                negativeNodes.Add(nonCombatNodes[i]);
                currentNodeCount++;
            }

            if (currentNodeCount >= totalEventNodes)
                break;
        }

        Debug.Log("Positive Count: " + positiveNodes.Count);
        Debug.Log("Neutral Count: " + neutralNodes.Count);
        Debug.Log("Negative Count: " + negativeNodes.Count);
    }
}
