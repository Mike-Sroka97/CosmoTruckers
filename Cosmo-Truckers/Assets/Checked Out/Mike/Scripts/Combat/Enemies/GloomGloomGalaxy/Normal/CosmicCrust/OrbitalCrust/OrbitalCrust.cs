using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCrust : CombatMove
{
    [SerializeField] OrbitalCrustStalatiteSpawner[] stalactiteSpawns;
    [SerializeField] float delayBetweenStalactites;
    [SerializeField] int maxStalactites = 25;

    int currentStalactites = 0;
    int lastRandom = -1;
    int random;

    private void Start()
    {
        StartMove();
        stalactiteSpawns = GetComponentsInChildren<OrbitalCrustStalatiteSpawner>();
        StartCoroutine(SpawnStalactite());
    }

    private void Update()
    {
        TrackTime();
    }

    public override void EndMove()
    {
        Score = (int)currentTime;
        base.EndMove();
    }

    IEnumerator SpawnStalactite()
    {
        yield return new WaitForSeconds(delayBetweenStalactites);

        currentStalactites++;

        random = UnityEngine.Random.Range(0, stalactiteSpawns.Length);

        while(random == lastRandom || stalactiteSpawns[random].IsFull)
        {
            random = UnityEngine.Random.Range(0, stalactiteSpawns.Length);
        }

        lastRandom = random;
        stalactiteSpawns[random].SpawnStalactite();

        if(!MoveEnded && currentStalactites < maxStalactites)
            StartCoroutine(SpawnStalactite());
    }
}
