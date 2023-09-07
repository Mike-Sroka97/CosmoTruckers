using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [HideInInspector] public Player MyPlayer;
    public string CharacterName { get; private set; }
    [SerializeField] GameObject wheel;
    [SerializeField] List<BaseAttackSO> attacks;
    List<BaseAttackSO> attackClones = new List<BaseAttackSO>();

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    private void Start()
    {
        foreach (DebuffStackSO augment in GetAUGS)
        {
            augment.MyCharacter = this;
            augment.DebuffEffect();
        }


        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

        turnOrder = FindObjectOfType<TurnOrder>();
        CurrentHealth = Health;
    }

    public override void StartTurn()
    {
        wheel.SetActive(true);
        wheel.GetComponentInChildren<AttackUI>().StartTurn(this);
    }
    public override void EndTurn()
    {
        wheel.SetActive(false);
    }

    public override void AdjustDefense(int defense)
    {
        Stats.Defense += defense;

        if (Stats.Defense > 50)
            Stats.Defense = 50;
        else if (Stats.Defense < -100)
            Stats.Defense = -100;
    }
}
