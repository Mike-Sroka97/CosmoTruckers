using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionableDeal : CombatMove
{
    [HideInInspector] public bool Moving = true;
    [SerializeField] int[] successThresholds;
    [SerializeField] float moveSpeed;

    bool trackTime = false;
    float currentTime = 0;
    Rigidbody2D myBody;

    const int SubductionDamage = 999;

    private void Start()
    {
        GenerateLayout();
        myBody = GetComponent<Rigidbody2D>();
    }

    public override void StartMove()
    {
        myBody.velocity = new Vector2(-moveSpeed, 0);
        GetComponent<ParentPlayer>().AdjustPlayerVelocity(myBody.velocity.x, myBody.velocity.y);
        trackTime = true;
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if (!trackTime)
            return;

        if(!PlayerDead)
        {
            currentTime += Time.deltaTime;
        }
        if(currentTime >= successThresholds[successThresholds.Length - 1] && Moving)
        {
            myBody.velocity = Vector2.zero;
            Moving = false;
            PlayerDead = true;
            Score = successThresholds.Length;
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if (Score > 0)
            AugmentScore++;

        //Calculate Damage
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage;
        //defending/attacking
        currentDamage = Score * Damage;
        currentDamage += baseDamage;

        //TODO CHANCE if enemy has subduction && !Enemy.IsBoss deal 999 damage instead. Use variable subductionDamage

        //Apply augment
        CombatManager.Instance.CharactersSelected[0].AddDebuffStack(DebuffToAdd, AugmentScore);

        if (currentDamage > 0)
            CombatManager.Instance.CharactersSelected[0].TakeDamage(currentDamage);

        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
