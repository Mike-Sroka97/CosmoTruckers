using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPatty : CombatMove
{
    [SerializeField] float timeBetweenHands;

    PotentPattyHand[] hands;
    float handTime = 1.4f; //helps get hands moving faster;
    bool trackTime = false;

    private void Start()
    {
        hands = GetComponentsInChildren<PotentPattyHand>();
        Score = GetComponentsInChildren<PotentPattyPatty>().Length;
    }

    public override void StartMove()
    {
        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }
    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        handTime += Time.deltaTime;

        if(handTime >= timeBetweenHands)
        {
            int random = UnityEngine.Random.Range(0, hands.Length);
            while(hands[random].Activated == true)
            {
                random = UnityEngine.Random.Range(0, hands.Length);
            }

            StartCoroutine(hands[random].Activate());
            currentTime = 0;
        }

        base.TrackTime();
    }

    public override void EndMove()
    {
        
    }
}
