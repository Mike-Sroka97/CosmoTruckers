using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShield : CombatMove
{
    [SerializeField] float xTeleportLimit;
    [SerializeField] float yTeleportLimit;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        SparkShieldCollectable[] collectables = GetComponentsInChildren<SparkShieldCollectable>();

        foreach (SparkShieldCollectable collectable in collectables)
            collectable.Initialize();

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        if (Score > maxScore)
            Score = maxScore;

        int shielding = baseDamage + Score * Damage;

        CombatManager.Instance.GetCurrentCharacter.TakeShielding(shielding);        
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} gaining {baseDamage + Score * Damage} shield. You gained {AugmentScore} stack(s) of {DebuffToAdd.AugmentName}.";
}
