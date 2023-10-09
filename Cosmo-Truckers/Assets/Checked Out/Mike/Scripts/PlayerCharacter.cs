using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [HideInInspector] public Player MyPlayer;
    [SerializeField] string Name;
    public bool IsDPS;
    public bool IsTank;
    public bool IsSupport;
    public bool IsUtility;
    public string CharacterName { get => Name; private set => Name = value; }
    [SerializeField] GameObject wheel;
    [SerializeField] List<BaseAttackSO> attacks;
    List<BaseAttackSO> attackClones = new List<BaseAttackSO>();

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    private void Start()
    {
        foreach (DebuffStackSO augment in AUGS)
            augment.MyCharacter = this;

        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

        turnOrder = FindObjectOfType<TurnOrder>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        CurrentHealth = Health;
    }

    public override void StartTurn()
    {
        FadeAugments();

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
