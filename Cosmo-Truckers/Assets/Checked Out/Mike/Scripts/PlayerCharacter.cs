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
    [SerializeField] GameObject augList;

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

    bool isTurn = false;

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

    private void Update()
    {
        if (!isTurn) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetupAttackWheel();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //Just this character for now to test out
            //TODO will take in any character based on targeting
            SetUpAUGDescription(this);
        }
    }

    public override void StartTurn()
    {
        isTurn = true;

        foreach (DebuffStackSO aug in AUGS)
        {
            if (aug.TurnStart)
            {
                aug.DebuffEffect();
            }
        }
    }

    public void SetupAttackWheel()
    {
        wheel.SetActive(true);
        wheel.GetComponentInChildren<AttackUI>().StartTurn(this);
    }
    public void SetUpAUGDescription(Character character)
    {
        //Just untill the augList is set for all characters
        if (augList == null) return;

        augList.SetActive(true);
        augList.GetComponent<UI_AUG_DESCRIPTION>().InitList(character);
    }
    public override void EndTurn()
    {
        wheel.SetActive(false);
        augList.SetActive(false);

        isTurn = false;
    }

    public override void AdjustDefense(int defense)
    {
        Stats.Defense += defense;

        if (Stats.Defense > 50)
            Stats.Defense = 50;
        else if (Stats.Defense < -100)
            Stats.Defense = -100;
    }

    public override void Resurrect(int newHealth, bool ignoreVigor = false)
    {
        base.Resurrect(newHealth, ignoreVigor);

        if (!ignoreVigor)
            newHealth = AdjustAttackHealing(newHealth);

        MyVessel.AdjustCurrentHealthDisplay(Health, newHealth, false);
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        base.TakeDamage(damage, defensePiercing);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        MyVessel.AdjustCurrentHealthDisplay(CurrentHealth, damage);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        MyVessel.AdjustMultiHitHealthDisplay(CurrentHealth, damage, numberOfHits);
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        base.TakeHealing(healing, ignoreVigor);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        MyVessel.AdjustCurrentHealthDisplay(CurrentHealth, healing, false);
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        base.TakeMultiHitHealing(healing, numberOfHeals, ignoreVigor);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        MyVessel.AdjustMultiHitHealthDisplay(CurrentHealth, healing, numberOfHeals, false);
    }


    public override void TakeShielding(int shieldAmount)
    {
        base.TakeShielding(shieldAmount);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        MyVessel.AdjustShieldDisplay(Shield, shieldAmount);
    }
}
