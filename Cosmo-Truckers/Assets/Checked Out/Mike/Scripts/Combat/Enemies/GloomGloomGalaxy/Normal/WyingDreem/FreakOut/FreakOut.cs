using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreakOut : CombatMove
{
    [SerializeField] float startDelay = 1f;

    FreakOutSpikeSpawner spawner;
    int lastRandom = -1;

    private void Start()
    {
        spawner = GetComponentInChildren<FreakOutSpikeSpawner>();
        Invoke("SpawnSpikes", startDelay);
    }

    public override void StartMove()
    {
        GetComponentInChildren<FreakOutSpikeSpawner>().enabled = true;
        trackTime = true;

        SetupMultiplayer();
    }

    private void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
    }

    public void SpawnSpikes()
    {
        bool[] spawns = new bool[9];
        int random = UnityEngine.Random.Range(0, spawns.Length - 1);

        while (lastRandom == random)
        {
            random = UnityEngine.Random.Range(0, spawns.Length - 1);
        }
        lastRandom = random;

        for(int i = 0; i < spawns.Length; i++)
        {
            if(i != random)
            {
                spawns[i] = true;
            }
            else
            {
                spawns[i] = false;
            }
        }

        spawner.SpawnSpikes(spawns);
    }

    public override void EndMove()
    {
        Debug.Log("lol");

        MoveEnded = true;

        int nitemareStacks = 0;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            int tempAugScore = PlayerAugmentScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()];
            nitemareStacks = CalculateMultiplayerAugmentScore(tempAugScore);

            if (i == 0)
                nitemareStacks++;

                 CombatManager.Instance.GetCharactersSelected[i].AddDebuffStack(DebuffToAdd, nitemareStacks);
        }
    }
}
