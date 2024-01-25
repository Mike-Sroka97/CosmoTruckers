using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarlightFury : CombatMove
{
    [SerializeField] float timeBetweenSwords;
    [SerializeField] int maxSwords;

    StarlightSword[] swords;
    int currentSwords = 0;

    private void Start()
    {
        trackTime = false;
        GenerateLayout();
        swords = GetComponentsInChildren<StarlightSword>();
    }

    public override void StartMove()
    {
        foreach (StarlightSword sword in swords)
            sword.Initialize();

        trackTime = true;
        StartCoroutine(NextSword());


        base.StartMove();
    }

    private void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
    }

    IEnumerator NextSword()
    {
        currentSwords++;

        int random = UnityEngine.Random.Range(0, swords.Length);

        while(swords[random].Activated)
        {
            random = UnityEngine.Random.Range(0, swords.Length);
        }

        StartCoroutine(swords[random].CCSword());

        yield return new WaitForSeconds(timeBetweenSwords);

        if(currentSwords < maxSwords && !PlayerDead)
        {
            StartCoroutine(NextSword());
        }
    }
}
