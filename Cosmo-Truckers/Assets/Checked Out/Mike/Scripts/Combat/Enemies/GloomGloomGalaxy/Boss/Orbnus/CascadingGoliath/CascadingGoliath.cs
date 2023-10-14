using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliath : CombatMove
{
    CascadingGoliathNodes[] allNodes;
    CascadingGoliathChunkSpawner mySpawner;
    [SerializeField] GameObject[] eyeBrows;
    [SerializeField] GameObject hairParticleEffect;
    [SerializeField] float phase2EyebrowWaitTime = 1f;

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
                hand.StopAllCoroutines(); 
                hand.StartCoroutine(hand.MoveOffScreen());
            }

            StartCoroutine(DestroyEyebrows()); 
        }
    }

    IEnumerator DestroyEyebrows()
    {
        if (eyeBrows != null)
        {
            yield return new WaitForSeconds(phase2EyebrowWaitTime);

            foreach (GameObject eyebrow in eyeBrows)
            {
                Instantiate(hairParticleEffect, eyebrow.transform.position, Quaternion.identity, null);
                Destroy(eyebrow);
            }
        }
    }
}
