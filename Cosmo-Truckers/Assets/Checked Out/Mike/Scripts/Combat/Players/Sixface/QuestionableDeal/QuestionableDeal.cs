using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionableDeal : CombatMove
{
    [HideInInspector] public bool Moving = true;
    [SerializeField] float moveSpeed;

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
        base.StartMove();
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        base.TrackTime();
        if(currentTime >= MinigameDuration - 3 && Moving)
        {
            myBody.velocity = Vector2.zero;
            GetComponent<ParentPlayer>().AdjustPlayerVelocity(myBody.velocity.x, myBody.velocity.y);
            Moving = false;
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

        //Check if not boss and has subduction for instant kill
        if(Score > 0 && !CombatManager.Instance.CharactersSelected[0].GetComponent<Enemy>().IsBoss)
            foreach (AugmentStackSO augment in CombatManager.Instance.CharactersSelected[0].GetAUGS)
                if (augment.DebuffName == "Subduction")
                    currentDamage = 999;

        //Apply augment
        CombatManager.Instance.CharactersSelected[0].AddAugmentStack(DebuffToAdd, AugmentScore);

        //1 being base damage
        float DamageAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

        CombatManager.Instance.CharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), pierces);

        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
