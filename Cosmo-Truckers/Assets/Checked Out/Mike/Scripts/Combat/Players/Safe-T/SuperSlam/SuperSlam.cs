using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlam : CombatMove
{
    private void Start()
    {
        GenerateLayout(); 
    }

    public override void StartMove()
    {
        FindObjectOfType<SSGozorMovement>().trackTime = true;

        SSGun[] ssGuns = FindObjectsOfType<SSGun>();
        foreach (SSGun ssGun in ssGuns)
            ssGun.trackTime = true;

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
