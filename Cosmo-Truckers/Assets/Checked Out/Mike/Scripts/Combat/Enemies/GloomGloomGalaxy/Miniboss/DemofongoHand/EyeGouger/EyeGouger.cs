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

    private void Start()
    {
        StartMove();
        Invoke("StartCycle", initialDelay);
    }

    private void Update()
    {
        TrackTime();
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
        if (FindObjectOfType<EyeGougerGem>())
            Destroy(FindObjectOfType<EyeGougerGem>());

        int random = UnityEngine.Random.Range(0, gemspawns.Length);

        while(random == lastGemSpawn || (firstGem && random == gemspawns.Length - 1))
        {
            random = UnityEngine.Random.Range(0, gemspawns.Length);
        }

        firstGem = false;

        GameObject newGem = Instantiate(gem, gemspawns[random]);
        newGem.transform.parent = null;
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
