using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJab : CombatMove
{
    [SerializeField] float timeBetweenShockAreaActivations;

    EnergonJabDVD[] dvds;
    List<Transform> spawnParents = new List<Transform>();
    List<Transform> coils = new List<Transform>();
    List<int> dvdsToActivate = new List<int>() { 0, 1, 2, 3 };

    private void Start()
    {
        GenerateLayout();
        dvds = GetComponentsInChildren<EnergonJabDVD>();

        Transform objectSpawns = GameObject.Find("ObjectSpawns").transform;
        foreach (Transform spawn in objectSpawns)
            spawnParents.Add(spawn);

        Transform coilParent = GameObject.Find("Coils").transform;
        foreach (Transform coil in coilParent)
            coils.Add(coil);

        RandomizeSpawnPositions();
        SetNextBall();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        ProtoMana mana = CombatManager.Instance.GetCurrentPlayer.GetComponent<ProtoMana>();

        if (mana.CurrentBattery == 0)
            mana.UpdateMana(CalculateManaGain());
    }

    private void RandomizeSpawnPositions()
    {
        for (int i = 0; i < 4; i++)
        {
            int randomDvdPosition = Random.Range(0, spawnParents[i].childCount);
            dvds[i].transform.position = spawnParents[i].GetChild(randomDvdPosition).position;

            // Don't let DVD and Coil spawn in the same location
            int randomCoilPosition = randomDvdPosition; 
            while (randomCoilPosition == randomDvdPosition)
                randomCoilPosition = Random.Range(0, spawnParents[i].childCount);

            coils[i].position = spawnParents[i].GetChild(randomCoilPosition).position;
        }
    }

    public void SetNextBall()
    {
        if (dvdsToActivate.Count > 0)
        {
            int random = dvdsToActivate[Random.Range(0, dvdsToActivate.Count)];
            dvds[random].ActivateMe();

            // Make it so you have to go to all four quadrants
            dvdsToActivate.Remove(random);
        }
    }

    private int CalculateManaGain()
    {
        if (Score >= maxScore)
            return 2;
        else if (Score >= maxScore / 2)
            return 1;
        else
            return 0;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {baseDamage + Score * Damage} damage. You gained {CalculateManaGain()} battery charges.";
}
