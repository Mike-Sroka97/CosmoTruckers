using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthyProcedure : CombatMove
{
    LPSuccess[] successNodes;
    List<GameObject> activatedNodes = new List<GameObject>();
    int random;

    private void Start()
    {
        random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform.Find("Layout"));

        successNodes = FindObjectsOfType<LPSuccess>();
        foreach(LPSuccess success in successNodes)
        {
            success.gameObject.SetActive(false);
        }
        random = UnityEngine.Random.Range(0, successNodes.Length);
        successNodes[random].gameObject.SetActive(true);
        activatedNodes.Add(successNodes[random].gameObject);
    }

    public override void StartMove()
    {
        LPPlatformMovement[] platforms = GetComponentsInChildren<LPPlatformMovement>();
        foreach (LPPlatformMovement platform in platforms)
            platform.StartMove();
    }

    public void NextNode()
    {
        if(activatedNodes.Count != successNodes.Length)
        {
            while (activatedNodes.Contains(successNodes[random].gameObject))
            {
                random = Random.Range(0, successNodes.Length);
            }
            successNodes[random].gameObject.SetActive(true);
            activatedNodes.Add(successNodes[random].gameObject);
        }
    }
}
