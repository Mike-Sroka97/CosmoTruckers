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
        StartCoroutine(StartSpawnSpikes());
        spawner = GetComponentInChildren<FreakOutSpikeSpawner>();
    }

    public override void StartMove()
    {
        spawner.enabled = true;
        SetupMultiplayer();

        base.StartMove();
    }

    private IEnumerator StartSpawnSpikes()
    {
        yield return new WaitForSeconds(startDelay);
        SpawnSpikes(); 
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
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        int nitemareStacks = 0;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            int tempAugScore = PlayerAugmentScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()];
            nitemareStacks = CalculateMultiplayerAugmentScore(tempAugScore);

            if (i == 0)
                nitemareStacks++;

                 CombatManager.Instance.GetCharactersSelected[i].AddAugmentStack(DebuffToAdd, nitemareStacks);
        }
    }
}
