using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : Character
{
    public string CharacterName { get => characterName; }

    [SerializeField] protected BaseAttackSO[] attacks;

    [Space(20)]
    [Header("Special Effects")]
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI healingText;
    public bool IsBoss = false;

    public BaseAttackSO[] GetAllAttacks { get => attacks; }
    public bool SpecialTargetConditions = false;
    [HideInInspector] public List<Character> CurrentTargets;
    [HideInInspector] public BaseAttackSO ChosenAttack;
    public bool TakesCombatSpot = true;

    [Header("Trash mob collector")]
    public bool IsTrash = false;
    [SerializeField] string characterName;

    Animator enemyAnimation;
    [SerializeField] protected AnimationClip deathAnimation; 

    private PlayerCharacter tauntedBy;
    protected UnityEvent tauntedByChanged = new UnityEvent();
    public PlayerCharacter TauntedBy
    {
        get
        {
            return tauntedBy;
        }

        set
        {
            tauntedBy = value;

            if(tauntedBy != null)
                tauntedByChanged.Invoke();
        }
    }

    private void Awake()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        turnOrder = FindObjectOfType<TurnOrder>();
        enemyAnimation = GetComponent<Animator>();
        CurrentHealth = Health;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        tauntedByChanged.RemoveAllListeners();
    }

    protected virtual void Start()
    {
        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnStartBattle)
            StartCoroutine(StartWait());

        QueueNextMove();
    }

    IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        passiveMove.Activate(this);
    }
    public override void Resurrect(int newHealth,  bool ignoreVigor = false)
    {
        if(EnemyManager.Instance.EnemyCombatSpots[CombatSpot] == this || !TakesCombatSpot)
            base.Resurrect(newHealth, ignoreVigor);
    }

    public override void StartTurn()
    {
        foreach (AugmentStackSO aug in AUGS)
        {
            if (aug.TurnStart)
            {
                aug.DebuffEffect();
            }
        }

        if (ChosenAttack == null)
            ChosenAttack = attacks[UnityEngine.Random.Range(0, attacks.Length)];

        CombatManager.Instance.StartTurnEnemy(ChosenAttack, this);
    }

    public override void EndTurn()
    {
        QueueNextMove();
        TauntedBy = null;
    }

    public override void AdjustDefense(int defense)
    {
        Stats.TrueDefense += defense;

        if (Stats.TrueDefense > 100)
            Stats.Defense = 100;
        else if (Stats.TrueDefense < -200)
            Stats.Defense = -200;
        else
            Stats.Defense = Stats.TrueDefense;
    }

    //Method to override for adding target cons to a move
    public void TargetConditions(BaseAttackSO currentAttack)
    {
        for(int i = 0; i < attacks.Length; i++)
        {
            if(attacks[i] == currentAttack)
            {
                SpecialTarget(i);
                return;
            }
        }
    }

    protected int GetAttackIndex()
    {
        for (int i = 0; i < attacks.Length; i++)
            if (attacks[i] == ChosenAttack)
                return i;

        return 0;
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        bool bubble = false;
        int originalDamage = damage; 

        if (BubbleShielded)
            bubble = true;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);
        if (bubble)
            damage = 0;

        StartCoroutine(DamageHealingEffect(true, damage.ToString(), EnumManager.CombatOutcome.Damage, originalDamage, defensePiercing));
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        bool bubble = false;
        int originalDamage = damage;

        if (BubbleShielded)
            bubble = true;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (bubble)
            numberOfHits--;

        StartCoroutine(DamageHealingEffect(true, damage.ToString(), EnumManager.CombatOutcome.MultiDamage, originalDamage, defensePiercing, numberOfHits));
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        int originalHealing = healing;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString(), EnumManager.CombatOutcome.Healing, originalHealing, ignoreVigor));
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        int originalHealing = healing;

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString(), EnumManager.CombatOutcome.MultiHealing, originalHealing, ignoreVigor, numberOfHeals));
    }

    public override void Energize(bool energize)
    {
        //I am not figuring out how to make trash mobs work with this function
        if(!IsTrash)
            base.Energize(energize);
    }

    protected virtual IEnumerator DamageHealingEffect(bool damage, string text, EnumManager.CombatOutcome outcome, int originalValue, bool piercing, int numberOfHits = 1)
    {
        CombatManager.Instance.CommandsExecuting++;

        // Fixes multilple damage/healing effect calls spawning it at same time. Can't use CommandsExecuting because it would mess up Multi-Target attacks
        while (LocalCommandsExecuting > 0)
            yield return null; 

        LocalCommandsExecuting++; 

        float finalStarAnimationWaitTime = 0f;

        for (int i = 0; i < numberOfHits; i++)
        {
            finalStarAnimationWaitTime = CallSpawnCombatStar(outcome, text, CombatStarsCurrentLayer);
            CombatStarsCurrentLayer++; 

            // Allow the stars to wait a small period of time between spawning each one
            yield return new WaitForSeconds(CombatManager.Instance.CombatStarSpawnWaitTime);
        }

        LocalCommandsExecuting--;

        // Wait for the final star to finish animating before actually dealing damage
        yield return new WaitForSeconds(finalStarAnimationWaitTime);

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
            default:
                break;
        }
    }

    public override void Die()
    {
        // An additional add to Commands Executing for Die
        CombatManager.Instance.CommandsExecuting++;

        // Start the Death Animation, which calls base.Die()
        StartCoroutine(DeathAnimation()); 
    }

    IEnumerator DeathAnimation()
    {
        float waitTime = 0; 

        if (deathAnimation != null)
        {
            enemyAnimation.Play(deathAnimation.name);
            waitTime = deathAnimation.length;
        }
        else { Debug.LogError("Warning: No Death animation assigned to enemy!"); }

        yield return new WaitForSeconds(waitTime);

        base.Die();
    }

    public void QueueNextMove()
    {
        if (EnemyManager.Instance.GetAlivePlayerCharacters().Count <= 0)
            return;

        SpecialTarget(SelectAttack());
    }

    //Reserved for enemies with no special AI
    protected virtual int SelectAttack()
    {
        if (attacks.Length <= 0)
            return -1;

        CurrentTargets.Clear();
        ChosenAttack = attacks[UnityEngine.Random.Range(0, attacks.Length)];
        CombatManager.Instance.EnemyTarget(ChosenAttack, this);
        return GetAttackIndex();
    }

    protected virtual void SpecialTarget(int attackIndex) { }

    //Always put IntentionChangeSpawn at the next level down from the parent for the love of god please I need this
    protected virtual void ChangeIntention()
    {
        Instantiate(CombatManager.Instance.IntentionChange, transform.Find("IntentionChangeSpawn"));
        QueueNextMove();
    }
}
