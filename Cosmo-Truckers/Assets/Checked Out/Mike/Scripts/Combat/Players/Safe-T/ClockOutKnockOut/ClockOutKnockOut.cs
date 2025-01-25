using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOutKnockOut : CombatMove
{
    public Material hurtMaterial;
    public Material noHurtMaterial;
    public float hurtOpacity;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        COKOhand[] hands = FindObjectsOfType<COKOhand>();

        foreach(COKOhand hand in hands)
        {
            hand.SetVelocity();
        }

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(1);
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage.";
}
