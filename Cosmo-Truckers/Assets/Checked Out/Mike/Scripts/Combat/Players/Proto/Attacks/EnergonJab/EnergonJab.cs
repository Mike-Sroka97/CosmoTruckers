using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJab : CombatMove
{
    [SerializeField] float timeBetweenShockAreaActivations;

    EnergonJabShockArea[] shockAreas;
    float currentTime = 2.5f;
    int lastRandom = -1;
    int lastlastRandom = -1;

    private void Start()
    {
        GenerateLayout();
        shockAreas = FindObjectsOfType<EnergonJabShockArea>();
        StartMove();
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;
        
        if(currentTime >= timeBetweenShockAreaActivations)
        {
            currentTime = 0;
            int random = UnityEngine.Random.Range(0, shockAreas.Length);

            while(random == lastRandom || random == lastlastRandom)
            {
                random = UnityEngine.Random.Range(0, shockAreas.Length);
            }

            lastlastRandom = lastRandom;
            lastRandom = random;

            shockAreas[random].ActivateMe();
        }
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
