using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPatty : CombatMove
{
    [SerializeField] float timeBetweenHands;

    PotentPattyHand[] hands;
    float handTime = 0f;
    bool trackTime = false;

    private void Start()
    {
        hands = GetComponentsInChildren<PotentPattyHand>();
        Score = GetComponentsInChildren<PotentPattyPatty>().Length;
        StartMoveTest(); 
    }

    public override void StartMove()
    {
        trackTime = true;
        FindObjectOfType<PotentPattyMech>().StartMove();
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
            handTime = 0;
        }

        base.TrackTime();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            //Calculate Damage
            if (Score < 0)
                Score = 0;
            if (Score >= maxScore)
                Score = maxScore;

            int currentHealing = 0;
            currentHealing = Score * Damage;
            currentHealing += baseDamage;

            character.Resurrect(currentHealing);
        }
    }
}
