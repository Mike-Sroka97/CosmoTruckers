using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGouger : CombatMove
{
    [SerializeField] Transform[] gemspawns;
    [SerializeField] GameObject gem;
    [SerializeField] EyeGougerHand[] hands;
    [SerializeField] int numberOfCycles;
    [SerializeField] float initialDelay;

    const int numberOfHandsPerCycle = 4;
    int currentNumberOfCycles = 0;
    int lastGemSpawn = -1;
    bool cycling = false;
    bool delay = true;
    bool firstGem = true;

    public override void StartMove()
    {
        base.StartMove();
        Invoke("StartCycle", initialDelay);

        base.StartMove();
    }

    protected override void Update()
    {
        base.Update();
        TrackCycle();
    }

    private void TrackCycle()
    {
        if (delay)
            return;

        bool allInactive = true;

        foreach(EyeGougerHand hand in hands)
        {
            if (hand.Active)
            {
                allInactive = false;
                break;
            }
        }

        if(allInactive)
        {
            StartCycle();
        }
    }

    public void StartCycle()
    {
        if (currentNumberOfCycles >= numberOfCycles || cycling)
            return;

        delay = false;
        cycling = true;
        currentNumberOfCycles++;

        SpawnGem();
        SetHands();

        cycling = false;
    }

    private void SpawnGem()
    {
        if (FindObjectOfType<PlayerPickup>())
            FindObjectOfType<PlayerPickup>().DestroyPickup();

        int random = UnityEngine.Random.Range(0, gemspawns.Length);

        while(random == lastGemSpawn || (firstGem && random == gemspawns.Length - 1))
        {
            random = UnityEngine.Random.Range(0, gemspawns.Length);
        }

        // The DOG might cause problems with this, so let's not get stuck in an infinite loop shall we
        int timesChecked = 0; 

        while ((!CanSpawnGem(gemspawns[random].position) || random == lastGemSpawn || (firstGem && random == gemspawns.Length - 1)) && timesChecked < 20)
        {
            random = UnityEngine.Random.Range(0, gemspawns.Length);
            timesChecked++; 
        }

        firstGem = false;
        lastGemSpawn = random; 

        Instantiate(gem, gemspawns[random]);
    }

    private bool CanSpawnGem(Vector2 spawnPoint)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint, 0.5f);

        // If it's the first star or it's tried to spawn over 10 times, just spawn it
        if (colliders.Length > 1)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                    return false;
            }
        }

        return true;
    }


    private void SetHands()
    {
        int currentNumberOfHandsSet = 0;

        while(currentNumberOfHandsSet < numberOfHandsPerCycle)
        {
            int random = UnityEngine.Random.Range(0, hands.Length);

            while(hands[random].Active == true)
            {
                random = UnityEngine.Random.Range(0, hands.Length);
            }

            hands[random].ActivateMe();
            currentNumberOfHandsSet++;
        }
    }
}
