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
    public Sprite VesselImage; //will be able to remove
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

    bool isTurn = false;
    bool checkingEnemyIntentions = false;

    private void Awake()
    {
        CurrentHealth = Health;
    }

    private void Start()
    {
        foreach (DebuffStackSO augment in passiveAugments)
            if (!AUGS.Contains(augment))
                AddDebuffStack(augment);

        foreach (DebuffStackSO augment in AUGS)
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

        foreach (DebuffStackSO aug in AUGS)
            if (aug.TurnStart)
                aug.DebuffEffect();

        foreach (DebuffStackSO augment in AugmentsToRemove)
            AdjustAugs(false, augment);

        AugmentsToRemove.Clear();
    }

    public void ClosePages()
    {
        wheel.SetActive(false);
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

        if (Stats.TrueDefense > 50)
            Stats.Defense = 50;
        else if (Stats.TrueDefense < -100)
            Stats.Defense = -100;
        else
            Stats.Defense = Stats.TrueDefense;
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
        bool bubble = false;

        if (BubbleShielded)
            bubble = true;

        base.TakeDamage(damage, defensePiercing);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);
        if (bubble)
            damage = 0;

        MyVessel.AdjustCurrentHealthDisplay(CurrentHealth, damage);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        bool bubble = false;

        if (BubbleShielded)
            bubble = true;

        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);

        if (GetComponent<PlayerCharacterSummon>())
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (bubble)
            numberOfHits--;

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
