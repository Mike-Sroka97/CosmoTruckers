using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Character
{
    [HideInInspector] public Player MyPlayer;
    [SerializeField] string Name;
    public Sprite VesselImage; //will be able to remove
    public bool IsDPS;
    public bool IsTank;
    public bool IsSupport;
    public bool IsUtility;
    public string CharacterName { get => Name; private set => Name = value; }
    [SerializeField] GameObject wheel;
    [SerializeField] List<BaseAttackSO> attacks;
    protected List<BaseAttackSO> attackClones = new List<BaseAttackSO>();

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    protected Mana manaBase;
    [HideInInspector] public PlayerVessel MyVessel;
    public GameObject PlayerVessel;
    public AttackUI PlayerAttackUI;

    private void Awake()
    {
        CurrentHealth = Health;
    }

    private void Start()
    {
        foreach (DebuffStackSO augment in AUGS)
            augment.MyCharacter = this;

        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

        manaBase = GetComponent<Mana>();
        turnOrder = FindObjectOfType<TurnOrder>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void StartTurn()
    {
        FadeAugments();

        SetupAttackWheel();
    }

    public void SetupAttackWheel()
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

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        base.TakeDamage(damage, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        MyVessel.AdjustCurrentHealthDisplay(CurrentHealth, damage);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);
        MyVessel.AdjustMultiHitHealthDisplay(CurrentHealth, damage, numberOfHits);
    }

    public override void TakeHealing(int healing)
    {
        base.TakeHealing(healing);
    }

    public override void TakeShielding(int shieldAmount)
    {
        base.TakeShielding(shieldAmount);
        MyVessel.AdjustShieldDisplay(Shield, shieldAmount);
    }
}
