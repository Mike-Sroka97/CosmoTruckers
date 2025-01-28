public class StarStorm : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override string TrainingDisplayText => $"You scored {Score = (maxScore - Score < 0 ? 0 : maxScore)}/{maxScore} taking {Score * Damage + baseDamage} damage. You also gained {AugmentScore * augmentStacksPerScore + baseAugmentStacks} stacks of {DebuffToAdd.AugmentName}.";
}
