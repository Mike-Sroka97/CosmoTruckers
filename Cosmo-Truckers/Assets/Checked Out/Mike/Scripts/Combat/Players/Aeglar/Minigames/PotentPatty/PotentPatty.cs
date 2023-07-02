using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPatty : CombatMove
{
    [SerializeField] float timeBetweenHands;

    PotentPattyHand[] hands;
    float currentTime = 1.4f; //sue me

    private void Start()
    {
        hands = GetComponentsInChildren<PotentPattyHand>();
        Score = GetComponentsInChildren<PotentPattyPatty>().Length;
        StartMove();
    }

    private void Update()
    {
        TrackTime();
    }
    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= timeBetweenHands)
        {
            int random = UnityEngine.Random.Range(0, hands.Length);
            while(hands[random].Activated == true)
            {
                random = UnityEngine.Random.Range(0, hands.Length);
            }

            StartCoroutine(hands[random].Activate());
            currentTime = 0;
        }
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
