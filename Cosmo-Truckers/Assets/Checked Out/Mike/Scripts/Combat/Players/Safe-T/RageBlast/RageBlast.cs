using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlast : CombatMove
{
    [SerializeField] RageBlastPlatform[] platforms;
    [SerializeField] float timeToDisablePlatform;
    [SerializeField] GameObject rbLoop;
    [SerializeField] float ringsPerSide = 7;
    [SerializeField] float minSpawnTime = .9f;
    [SerializeField] float maxSpawnTime = 1.25f;
    [SerializeField] float minSpawnHeight = -0.5f;
    [SerializeField] float midSpawnHeight = 0f;
    [SerializeField] float maxSpawnHeight = 0.5f;
    [SerializeField] float xSpawn = 6.5f;

    int lastNumber = -1;
    int nonDuplicateRandom;
    float currentRightWaitTime = 0f;
    float currentLeftWaitTime = 0f;
    float rightWaitTime;
    float leftWaitTime;
    int leftSpawned;
    int rightSpawned;
    bool trackPlatformTime = true;

    private void Start()
    {
        nonDuplicateRandom = lastNumber;

        rightWaitTime = Random.Range(minSpawnTime, maxSpawnTime);
        leftWaitTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        base.TrackTime();
        TrackRingTime();
        TrackPlatformTime();
    }

    private void TrackPlatformTime()
    {
        if (currentTime >= timeToDisablePlatform && trackPlatformTime)
        {
            trackPlatformTime = false;
            NextPlatform();
        }
    }

    private void TrackRingTime()
    {
        currentLeftWaitTime += Time.deltaTime;
        currentRightWaitTime += Time.deltaTime;

        if(currentLeftWaitTime >= leftWaitTime && leftSpawned < ringsPerSide)
        {
            currentLeftWaitTime = 0;
            leftWaitTime = Random.Range(minSpawnTime, maxSpawnTime);
            RageBlastLoop loop = Instantiate(rbLoop, new Vector3(-xSpawn, GetRandomHeight(), 0), transform.rotation, transform).GetComponent<RageBlastLoop>();
            loop.InitializeLoop(false);

            leftSpawned++;
        }
        else if(currentRightWaitTime >= rightWaitTime && rightSpawned < ringsPerSide)
        {
            currentRightWaitTime = 0;
            rightWaitTime = Random.Range(minSpawnTime, maxSpawnTime);
            RageBlastLoop loop = Instantiate(rbLoop, new Vector3(xSpawn, GetRandomHeight(), 0), transform.rotation, transform).GetComponent<RageBlastLoop>();
            loop.InitializeLoop(true);

            rightSpawned++;
        }
    }

    private float GetRandomHeight()
    {
        int random = Random.Range(0, 3);

        switch(random)
        {
            case (0):
                return maxSpawnHeight;
            case (1):
                return midSpawnHeight;
            case (2):
                return minSpawnHeight;
            default:
                return maxSpawnHeight;
        }
    }

    public void NextPlatform()
    {
        while(lastNumber == nonDuplicateRandom)
        {
            lastNumber = UnityEngine.Random.Range(0, platforms.Length);
        }

        nonDuplicateRandom = lastNumber;
        platforms[nonDuplicateRandom].DisableMe();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(CombatManager.Instance.GetCharactersSelected.Count);
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage to all enemies.";
}
