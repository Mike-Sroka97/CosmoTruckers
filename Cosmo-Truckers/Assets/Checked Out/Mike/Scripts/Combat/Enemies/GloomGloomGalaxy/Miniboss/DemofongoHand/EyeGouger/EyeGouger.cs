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
            Destroy(FindObjectOfType<PlayerPickup>());

        int random = UnityEngine.Random.Range(0, gemspawns.Length);

        while(random == lastGemSpawn || (firstGem && random == gemspawns.Length - 1))
        {
            random = UnityEngine.Random.Range(0, gemspawns.Length);
        }

        firstGem = false;

        Instantiate(gem, gemspawns[random]);
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
