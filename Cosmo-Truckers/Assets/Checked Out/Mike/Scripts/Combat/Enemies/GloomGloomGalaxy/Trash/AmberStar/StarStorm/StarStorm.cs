public class StarStorm : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override string TrainingDisplayText => $"You scored {GetFinalEnemyScore()}/{maxScore} taking {GetFinalScore() * Damage + baseDamage} damage. You also gained {GetFinalAugmentStackCount()} stacks of {DebuffToAdd.AugmentName}.";
}
