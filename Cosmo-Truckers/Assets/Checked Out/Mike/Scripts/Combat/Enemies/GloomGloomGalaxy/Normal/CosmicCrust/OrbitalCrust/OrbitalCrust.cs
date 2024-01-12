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
        stalactiteSpawns = GetComponentsInChildren<OrbitalCrustStalatiteSpawner>();
    }

    public override void StartMove()
    {
        base.StartMove();
        StartCoroutine(SpawnStalactite());
    }

    private void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
    }

    public override void EndMove()
    {
        int damage = CalculateScore();
        int stacks = CalculateAugmentScore();

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
        ApplyAugment(CombatManager.Instance.GetCurrentEnemy, stacks);
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
