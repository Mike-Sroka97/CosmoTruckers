using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShock : CombatMove
{
    [SerializeField] Vector4 teleportBounds;

    [SerializeField] Transform[] zappers;
    [SerializeField] GameObject zap;
    [SerializeField] float zapDelay;

    SystemShockHittable[] hittables;
    ProtoINA proto;
    float currentTime = 0;
    int lastRandom = -1;
    int lastRandomAgain = -1;
    int lastHittableRandom = -1;
    int lastHittableRandomAgain = -1;

    private void Start()
    {
        hittables = FindObjectsOfType<SystemShockHittable>();
        proto = FindObjectOfType<ProtoINA>();
        proto.SetTelportBoundaries(teleportBounds.x, teleportBounds.y, teleportBounds.z, teleportBounds.w);
        GenerateHittables();
        StartMove();
    }

    private void Update()
    {
        TrackScore();
        TrackTime();
    }

    private void TrackScore()
    {

        int hittablesHit = 0;
        foreach(SystemShockHittable hittable in hittables)
        {
            if (hittable.Hit)
                hittablesHit++;
        }

        if(hittablesHit >= 2)
        {
            GenerateHittables();
        }
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= zapDelay)
        {
            currentTime = 0;

            int randomOne = UnityEngine.Random.Range(0, zappers.Length);

            while(randomOne == lastRandom || randomOne == lastRandomAgain)
            {
                randomOne = UnityEngine.Random.Range(0, zappers.Length);
            }

            int randomTwo = UnityEngine.Random.Range(0, zappers.Length);

            while (randomTwo == lastRandom || randomTwo == lastRandomAgain || randomTwo == randomOne)
            {
                randomTwo = UnityEngine.Random.Range(0, zappers.Length);
            }

            lastRandom = randomOne;
            lastRandomAgain = randomTwo;

            Instantiate(zap, zappers[randomOne]);
            Instantiate(zap, zappers[randomTwo]);
        }
    }

    private void GenerateHittables()
    {
        foreach(SystemShockHittable hittable in hittables)
        {
            hittable.DeactivateMe();
        }

        int randomOne = UnityEngine.Random.Range(0, hittables.Length);

        while(randomOne == lastHittableRandom || randomOne == lastHittableRandomAgain)
        {
            randomOne = UnityEngine.Random.Range(0, hittables.Length);
        }

        int randomTwo = UnityEngine.Random.Range(0, hittables.Length);

        while(randomTwo == lastHittableRandom || randomTwo == lastHittableRandomAgain || randomTwo == randomOne)
        {
            randomTwo = UnityEngine.Random.Range(0, hittables.Length);
        }

        lastHittableRandom = randomOne;
        lastHittableRandomAgain = randomTwo;

        hittables[randomOne].ActivateMe();
        hittables[randomTwo].ActivateMe();
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
