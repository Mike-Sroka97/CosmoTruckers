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
    int lastRandom = -1;
    int lastRandomAgain = -1;
    int lastHittableRandom = -1;
    int lastHittableRandomAgain = -1;
    public int HittablesHit = 0;
    float currentTurretTime = 0;
    bool initialized = false;

    private void Start()
    {
        // Make sure this isn't enabled it will kill the player immediately
        FindObjectOfType<Rotator>().enabled = false;
        hittables = FindObjectsOfType<SystemShockHittable>();
        GenerateHittables();
    }

    public override void StartMove()
    {
        SystemShockTurret[] turrets = FindObjectsOfType<SystemShockTurret>();
        foreach (SystemShockTurret turret in turrets)
            turret.Initialize();
        FindObjectOfType<Rotator>().enabled = true;

        initialized = true;

        base.StartMove();
    }

    private void Update()
    {
        if (!initialized)
            return;

        TrackScore();
        TrackTime();
    }

    private void TrackScore()
    {
        if(HittablesHit >= 2)
        {
            HittablesHit = 0;
            GenerateHittables();
        }
    }

    protected override void TrackTime()
    {
        base.TrackTime();

        currentTurretTime += Time.deltaTime;

        if(currentTurretTime >= zapDelay)
        {
            currentTurretTime = 0;

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
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        //mana stuff
        ProtoMana mana = FindObjectOfType<ProtoMana>();
        if (AugmentScore > maxAugmentStacks)
            AugmentScore = maxAugmentStacks;

        int retention = AugmentScore / 3;
        if (retention > 2)
            retention = 2;
        mana.AdjustRetention(retention); //3 because it is 1/3 of the debuff stacks to give out
    }

    public override string TrainingDisplayText => $"You scored {AugmentScore = (AugmentScore > maxAugmentStacks ? maxAugmentStacks : AugmentScore)}/{maxAugmentStacks} giving your target {AugmentScore} stack(s) of {DebuffToAdd.AugmentName}. You retained {(AugmentScore / 3 > 2 ? 2 : AugmentScore / 3)} battery charges.";
}
