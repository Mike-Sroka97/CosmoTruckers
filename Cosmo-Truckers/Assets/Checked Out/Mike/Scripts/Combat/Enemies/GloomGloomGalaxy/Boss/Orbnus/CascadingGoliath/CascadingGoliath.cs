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
    public Material OffMaterial;

    private void Start()
    {
        GenerateLayout();

        mySpawner = GetComponent<CascadingGoliathChunkSpawner>();
        allNodes = FindObjectsOfType<CascadingGoliathNodes>();
    }

    public override void StartMove()
    {
        foreach (CascadingGoliathChunkSpawner chunkSpawner in GetComponentsInChildren<CascadingGoliathChunkSpawner>())
            chunkSpawner.enabled = true;
        foreach (CascadingGoliathHand hand in GetComponentsInChildren<CascadingGoliathHand>())
            hand.enabled = true;

        SetupMultiplayer();
    }

    public void CheckPhase()
    {
        bool phaseTwo = true;

        foreach (CascadingGoliathNodes node in allNodes)
        {
            if (!node.Hit)
            {
                phaseTwo = false;
            }
        }

        if (phaseTwo)
        {
            mySpawner.PhaseOne = false;

            CascadingGoliathHand[] hands = FindObjectsOfType<CascadingGoliathHand>();

            foreach (CascadingGoliathHand hand in hands)
            {
                foreach (TrackPlayerDeath tpd in hand.GetComponentsInChildren<TrackPlayerDeath>())
                    tpd.enabled = false;
                foreach (SpriteRenderer renderer in hand.GetComponentsInChildren<SpriteRenderer>())
                    renderer.material = OffMaterial;
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

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        if (FightWon)
            FindObjectOfType<OrbnusAI>().DieForReal();
    }
}


