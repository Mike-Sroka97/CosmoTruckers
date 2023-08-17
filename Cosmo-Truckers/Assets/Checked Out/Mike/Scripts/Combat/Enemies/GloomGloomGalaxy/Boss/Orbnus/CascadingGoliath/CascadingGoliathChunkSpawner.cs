using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CascadingGoliathChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject orbnusChunk;
    [SerializeField] Transform[] chunkSpawns;
    [SerializeField] float phaseOneDelay;
    [SerializeField] float phaseTwoDelay;
    [SerializeField] float phaseTwoChunkCount;

    [HideInInspector] public bool PhaseOne = true;
    int random;
    int lastRandom = -1;
    int currentChunk = 0;

    private void Start()
    {
        StartCoroutine(SpawnChunk());
    }

    IEnumerator SpawnChunk()
    {
        if (PhaseOne)
            yield return new WaitForSeconds(phaseOneDelay);
        else
            yield return new WaitForSeconds(phaseTwoDelay);

        random = Random.Range(0, chunkSpawns.Length);

        while(random == lastRandom)
        {
            random = Random.Range(0, chunkSpawns.Length);
        }

        lastRandom = random;

        Instantiate(orbnusChunk, chunkSpawns[random].position, orbnusChunk.transform.rotation);

        if (!PhaseOne)
            currentChunk++;

        if(currentChunk < phaseTwoChunkCount)
        {
            StartCoroutine(SpawnChunk());
        }
    }
}
