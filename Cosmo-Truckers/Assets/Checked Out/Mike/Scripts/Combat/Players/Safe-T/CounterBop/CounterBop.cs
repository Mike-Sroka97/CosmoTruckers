using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBop : CombatMove
{
    [SerializeField] CounterBopJumpCheck[] jumpChecks;

    CounterBopFace safeTFace; 
    List<int> usedJumpChecks = new List<int>();
    private int charges = 0;

    private void Start()
    { 
        safeTFace = FindObjectOfType<CounterBopFace>();
        GenerateNextBall();
    }

    private void GenerateNextBall()
    {
        if (charges < maxScore)
        {
            int random = Random.Range(0, jumpChecks.Length);

            while (usedJumpChecks.Contains(random))
                random = Random.Range(0, jumpChecks.Length);

            usedJumpChecks.Add(random);
            jumpChecks[random].EnableBalls();
        }
    }

    public void IncrementScore()
    {
        if (charges < maxScore)
        {
            safeTFace.IncrementScore(charges);
            charges++;
            GenerateNextBall();
        }
    }

    public void HitBall()
    {
        Score = charges;
        CheckSuccess();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        SafeTCharacter character = CombatManager.Instance.GetCurrentPlayer.GetComponent<SafeTCharacter>();

        //Calculate total shields
        int totalShields = baseDamage;
        totalShields += Damage * Score;

        character.TakeShielding(totalShields);

        //Add CountR Guard augment
        character.AddAugmentStack(DebuffToAdd, baseAugmentStacks);

        //Taunt random enemy
        List<Enemy> aliveEnemies = new List<Enemy>();

        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            if (!enemy.Dead)
                aliveEnemies.Add(enemy);

        int random = Random.Range(0, aliveEnemies.Count);
        aliveEnemies[random].TauntedBy = character;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} gaining {Score * Damage + baseDamage} shield and gaining {baseAugmentStacks} of {DebuffToAdd.AugmentName}. A random enemy was taunted too.";
}
