using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliath : CombatMove
{
    CascadingGoliathNodes[] allNodes;
    CascadingGoliathChunkSpawner mySpawner;

    private void Start()
    {
        StartMove();
        GenerateLayout();

        mySpawner = GetComponent<CascadingGoliathChunkSpawner>();
        allNodes = FindObjectsOfType<CascadingGoliathNodes>();
    }

    public void CheckPhase()
    {
        bool phaseTwo = true;

        foreach(CascadingGoliathNodes node in allNodes)
        {
            if(!node.Hit)
            {
                phaseTwo = false;
            }
        }

        if(phaseTwo)
        {
            mySpawner.PhaseOne = false;
            CascadingGoliathHand[] hands = FindObjectsOfType<CascadingGoliathHand>();

            foreach(CascadingGoliathHand hand in hands)
            {
                hand.enabled = false;
            }
        }
    }
}
