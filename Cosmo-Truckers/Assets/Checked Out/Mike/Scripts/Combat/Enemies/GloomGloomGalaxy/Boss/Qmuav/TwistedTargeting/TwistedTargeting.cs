using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TwistedTargeting : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        SetupMultiplayer();

        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        StartCoroutine(GetComponentInChildren<QmuavProjectileDelay>().SpawnWave());

        base.StartMove();
    }

    public override void EndMove()
    {
        //swap positions
        List<Character> playerHealths = CombatManager.Instance.GetCharactersSelected.OrderBy(character => character.Health).ToList();

        for (int i = 0; i < playerHealths.Count; i++)
            playerHealths[i].FlipCharacter(i, true);

        base.EndMove();
    }
}
