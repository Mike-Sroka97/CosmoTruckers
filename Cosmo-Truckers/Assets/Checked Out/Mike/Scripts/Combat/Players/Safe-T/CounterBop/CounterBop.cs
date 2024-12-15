using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBop : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        CBCircleEnemy[] circleEnemies = FindObjectsOfType<CBCircleEnemy>();
        CBStraightEnemy[] straightEnemies = FindObjectsOfType<CBStraightEnemy>();
        CBWaveEnemy[] waveEnemies = FindObjectsOfType<CBWaveEnemy>();

        foreach (CBCircleEnemy circleEnemy in circleEnemies)
            circleEnemy.enabled = true;
        foreach (CBStraightEnemy straightEnemy in straightEnemies)
            straightEnemy.enabled = true;
        foreach (CBWaveEnemy waveEnemy in waveEnemies)
            waveEnemy.enabled = true;

        base.StartMove();
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
