using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBop : CombatMove
{
    [SerializeField] DebuffStackSO counterGuardAUG;

    private void Start()
    {
        GenerateLayout();
        StartMoveTest();
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
    }

    public override void EndMove()
    {
        SafeTCharacter character = CombatManager.Instance.GetCurrentPlayer.GetComponent<SafeTCharacter>();

        //Calculate total shields
        int totalShields = baseDamage;
        for (int i = 0; i < Score; i++)
            totalShields += Damage;

        character.TakeShielding(totalShields);

        //Add CountR Guard augment
        character.AddDebuffStack(counterGuardAUG);
    }
}
