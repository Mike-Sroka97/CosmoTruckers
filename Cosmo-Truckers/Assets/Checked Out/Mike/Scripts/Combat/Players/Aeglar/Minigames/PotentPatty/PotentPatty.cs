using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPatty : CombatMove
{
    [SerializeField] float timeBetweenHands;

    PotentPattyHand[] hands;
    float handTime = 0f;

    private void Start()
    {
        hands = GetComponentsInChildren<PotentPattyHand>();
        Score = GetComponentsInChildren<PotentPattyPatty>().Length; 
    }

    public override void StartMove()
    {
        trackTime = true;
        FindObjectOfType<PotentPattyMech>().StartMove();

        base.StartMove();
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
        if (FindObjectOfType<InaPractice>())
            return;

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

            //1 being base damage
            float HealingAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

            float tempHealing = currentHealing * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment;
            currentHealing = (int)tempHealing;

            character.Resurrect(currentHealing);
        }
    }
}
