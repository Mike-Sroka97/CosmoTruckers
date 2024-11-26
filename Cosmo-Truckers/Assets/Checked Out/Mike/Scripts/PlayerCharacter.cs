using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCharacter : Character
{
    [HideInInspector] public Player MyPlayer;
    [SerializeField] string Name;
    public bool IsDPS;
    public bool IsTank;
    public bool IsSupport;
    public bool IsUtility;
    public bool RevokeControls = false;
    public int PlayerNumber;
    public string CharacterName { get => Name; private set => Name = value; }

    [Header("Start turn objects")]
    public UI_PlayerTurnStart SelectionUI;
    [SerializeField] GameObject wheel;
    [SerializeField] GameObject augList;

    [SerializeField] protected List<BaseAttackSO> attacks;
    protected List<BaseAttackSO> attackClones = new List<BaseAttackSO>();

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    protected Mana manaBase;
    public Mana GetManaBase { get => manaBase; }
    [HideInInspector] public PlayerVessel MyVessel;
    public GameObject PlayerVessel;
    public AttackUI PlayerAttackUI;

    public UnityEvent AttackWheelOpenedEvent = new UnityEvent();
    public UnityEvent AUGListOpenedEvent = new UnityEvent();
    public UnityEvent AUGListClosedEvent = new UnityEvent();
    public UnityEvent InsightOpenedEvent = new UnityEvent();

    bool isTurn = false;
    bool checkingEnemyIntentions = false;

    private void Awake()
    {
        CurrentHealth = Health;
    }

    private void Start()
    {
        foreach (AugmentStackSO augment in passiveAugments)
            if (!AUGS.Contains(augment))
                AddAugmentStack(augment);

        foreach (AugmentStackSO augment in AUGS)
            augment.MyCharacter = this;

        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

        manaBase = GetComponent<Mana>();
        turnOrder = FindObjectOfType<TurnOrder>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private int playerIntent = 0;
    private int lastIntent = 0;
    private int maxPlayerIntent = 2;

    private void Update()
    {
        if (!isTurn || RevokeControls) return;

        if(wheel.activeInHierarchy || augList.activeInHierarchy || checkingEnemyIntentions)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CombatManager.Instance.AttackDescription.gameObject.activeInHierarchy && !checkingEnemyIntentions)
                    return;

                ClosePages();
                SelectionUI.gameObject.SetActive(true);
                SetPlayerCurrentOption();

                if(checkingEnemyIntentions)
                {
                    checkingEnemyIntentions = false;
                    CombatManager.Instance.AttackDisplay.StartClose();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                lastIntent = playerIntent;

                if (playerIntent >= maxPlayerIntent)
                    playerIntent = 0;
                else
                    playerIntent++;

                SetPlayerCurrentOption();
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                lastIntent = playerIntent;

                if (playerIntent == 0)
                    playerIntent = maxPlayerIntent;
                else
                    playerIntent--;

                SetPlayerCurrentOption();
            }
            else if(Input.GetKeyDown(KeyCode.Space))
            {
                SelectionUI.gameObject.SetActive(false);

                switch (playerIntent)
                {
                    //Attack
                    case 0:
                        SetupAttackWheel();
                        AttackWheelOpenedEvent.Invoke();
                        break;
                    //Augs
                    case 1:
                        SetUpAUGDescription(this);
                        AUGListOpenedEvent.Invoke();
                        break;
                    //Intentions
                    case 2:
                        checkingEnemyIntentions = true;
                        CombatManager.Instance.MyTargeting.StartCheckingEnemyIntentions();
                        InsightOpenedEvent.Invoke();
                        break;
                    default: break;
                }
            }
        }
    }

    protected override void OnDisable()
    {
        //Clean up
        AttackWheelOpenedEvent.RemoveAllListeners();
        AUGListOpenedEvent.RemoveAllListeners();
        AUGListClosedEvent.RemoveAllListeners();
        PlayerAttackUI.AttackSelected.RemoveAllListeners();
        InsightOpenedEvent.RemoveAllListeners(); 

        base.OnDisable();
    }

    public override void StartTurn()
    {
        isTurn = true;
        SelectionUI.gameObject.SetActive(true);

        if (SelectionUI.AttackDisabled)
            playerIntent = 1;
        if (SelectionUI.AttackDisabled && SelectionUI.AugDisabled)
            playerIntent = 2;

        SetPlayerCurrentOption();

        CombatManager.Instance.CurrentCharacter = this;

        foreach (AugmentStackSO aug in AUGS)
            if (aug.TurnStart)
                aug.AugmentEffect();

        foreach (AugmentStackSO augment in AugmentsToRemove)
            AdjustAugs(false, augment);

        AugmentsToRemove.Clear();
    }

    public void ClosePages()
    {
        wheel.SetActive(false);

        if (augList.activeInHierarchy)
            AUGListClosedEvent.Invoke(); 
        
        augList.SetActive(false);

        SelectionUI.ResetColor();
        CombatManager.Instance.AttackDescription.gameObject.SetActive(false);
    }

    void SetPlayerCurrentOption()
    {
        switch(playerIntent)
        {
            //Attack
            case 0:
                if (SelectionUI.AttackDisabled)
                {
                    playerIntent = lastIntent;
                    return;
                }

                SelectionUI.StartAttack();
                break;
            //Augs
            case 1:
                if (SelectionUI.AugDisabled)
                {
                    playerIntent = lastIntent;
                    return;
                }

                SelectionUI.StartAUG();
                break;
            //Intentions
            case 2:
                if (SelectionUI.InsightDisabled)
                {
                    playerIntent = lastIntent;
                    return;
                }
;
                SelectionUI.StartIntent();
                break;
            default: break;
        }
    }
    public void SetupAttackWheel()
    {
        isTurn = true;
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

    public override void AdjustMaxHealth(int adjuster)
    {
        base.AdjustMaxHealth(adjuster);

        MyVessel.AdjustFill();
    }
    public override void AdjustDefense(int defense)
    {
        Stats.TrueDefense += defense;

        if (Stats.TrueDefense > 90)
            Stats.Defense = 90;
        else if (Stats.TrueDefense < -100)
            Stats.Defense = -100;
        else
            Stats.Defense = Stats.TrueDefense;
    }

    public override void Resurrect(int newHealth, bool ignoreVigor = false)
    {
        if (!ignoreVigor)
            newHealth = AdjustAttackHealing(newHealth);

        MyVessel.StartCoroutine(MyVessel.DamageHealingEffect(false, newHealth, EnumManager.CombatOutcome.Resurrect));
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        bool bubble = false;

        if (BubbleShielded)
            bubble = true;

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);
        if (bubble)
            damage = 0;


        MyVessel.StartCoroutine(MyVessel.DamageHealingEffect(true, damage, EnumManager.CombatOutcome.Damage));
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        bool bubble = false;

        if (BubbleShielded)
            bubble = true;

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (bubble)
            numberOfHits--;

        MyVessel.StartCoroutine(MyVessel.DamageHealingEffect(true, damage, EnumManager.CombatOutcome.MultiDamage, numberOfHits));
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        MyVessel.StartCoroutine(MyVessel.DamageHealingEffect(false, healing, EnumManager.CombatOutcome.Healing));
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        MyVessel.StartCoroutine(MyVessel.DamageHealingEffect(false, healing, EnumManager.CombatOutcome.MultiHealing, numberOfHeals));
    }

    public override void TakeShielding(int shieldAmount)
    {
        if (GetComponent<PlayerCharacterSummon>())
            return;

        MyVessel.StartCoroutine(MyVessel.ShieldEffect(shieldAmount)); 
    }

    public virtual void SwitchCombatOutcomes(EnumManager.CombatOutcome outcome, int originalValue, bool piercing, int numberOfHits = 1)
    {
        // Call the Damage / Healing base method after so death occurs visually after damage numbers have shown up
        switch (outcome)
        {
            case EnumManager.CombatOutcome.Damage:
                base.TakeDamage(originalValue, piercing);
                break;
            case EnumManager.CombatOutcome.Healing:
                base.TakeHealing(originalValue, piercing);
                break;
            case EnumManager.CombatOutcome.MultiDamage:
                base.TakeMultiHitDamage(originalValue, numberOfHits, piercing);
                break;
            case EnumManager.CombatOutcome.MultiHealing:
                base.TakeMultiHitHealing(originalValue, numberOfHits, piercing);
                break;
            case EnumManager.CombatOutcome.Resurrect:
                base.Resurrect(originalValue, piercing);
                break;
            case EnumManager.CombatOutcome.Shield:
                base.TakeShielding(originalValue);
                break; 
            default:
                break;
        }

        MyVessel.UpdateHealthText();
    }
}
