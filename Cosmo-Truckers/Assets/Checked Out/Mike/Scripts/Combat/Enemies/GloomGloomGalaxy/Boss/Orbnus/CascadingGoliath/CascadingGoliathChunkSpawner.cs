using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CascadingGoliathChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] orbnusChunk;
    [SerializeField] Transform[] chunkSpawns;
    [SerializeField] float phaseOneDelay;
    [SerializeField] float phaseTwoDelay;
    [SerializeField] float phaseTwoChunkCount;
    [SerializeField] GameObject noseChunk;
    [SerializeField] float noseWaitTime;
    [SerializeField] float minChunkRotation;
    [SerializeField] float maxChunkRotation;

    [HideInInspector] public bool PhaseOne = true;
    int random;
    int lastRandom = -1;
    int currentChunk = 0;
    bool finalNodeSpawned = false;

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

        while (random == lastRandom)
        {
            random = Random.Range(0, chunkSpawns.Length);
        }

        lastRandom = random;

        RotateChunks();

        if (!PhaseOne)
            currentChunk++;

        if (currentChunk < phaseTwoChunkCount)
        {
            StartCoroutine(SpawnChunk());
        }
        else if (!finalNodeSpawned)
        {
            finalNodeSpawned = true;
            yield return new WaitForSeconds(noseWaitTime);
            Instantiate(noseChunk, new Vector3(0, chunkSpawns[random].position.y, 0), noseChunk.transform.rotation);
        }
    }

    private void RotateChunks()
    {
        int chunkToSpawn = Random.Range(0, orbnusChunk.Length);
        Rotator rotator = Instantiate(orbnusChunk[chunkToSpawn], chunkSpawns[random].position, Quaternion.identity).GetComponent<Rotator>();

        rotator.RotateSpeed = Random.Range(minChunkRotation, maxChunkRotation);

        if (rotator.RotateSpeed > -100 && rotator.RotateSpeed < 100)
        {
            if (rotator.RotateSpeed <= 0)
            {
                rotator.RotateSpeed = -100;
            }
            else if (rotator.RotateSpeed > 0)
            {
                rotator.RotateSpeed = 100;
            }
        }
    }
}
