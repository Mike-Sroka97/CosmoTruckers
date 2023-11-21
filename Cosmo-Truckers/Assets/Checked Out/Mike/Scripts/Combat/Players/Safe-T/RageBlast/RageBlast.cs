using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlast : CombatMove
{
    [SerializeField] RageBlastPlatform[] platforms;
    [SerializeField] float timeToDisablePlatform;

    bool trackTime = false;
    int lastNumber = -1;
    int nonDuplicateRandom;

    private void Start()
    {
        nonDuplicateRandom = lastNumber;
    }

    public override void StartMove()
    {
        GenerateLayout();
        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }

    protected override void TrackTime()
    {
        if(trackTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeToDisablePlatform)
            {
                trackTime = false;
                NextPlatform();
            }
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
        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(CombatManager.Instance.GetCharactersSelected.Count);
    }
}
