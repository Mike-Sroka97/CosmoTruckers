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
    [SerializeField] Color damageColor;
    [SerializeField] Color healingColor;
    public bool IsBoss = false;
    Vector3 damageTextStartPosition;
    Vector3 healingTextStartPosition;

    public BaseAttackSO[] GetAllAttacks { get => attacks; }
    public bool SpecialTargetConditions = false;
    [HideInInspector] public List<Character> CurrentTargets;
    [HideInInspector] public BaseAttackSO ChosenAttack;
    public bool TakesCombatSpot = true;

    [Header("Trash mob collector")]
    public bool IsTrash = false;
    [SerializeField] string characterName;

    Animator enemyAnimation;

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
            tauntedBy = TauntedBy;

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
        foreach (DebuffStackSO aug in AUGS)
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

        if (BubbleShielded)
            bubble = true;

        base.TakeDamage(damage, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);
        if (bubble)
            damage = 0;

        StartCoroutine(DamageHealingEffect(true, damage.ToString()));
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        bool bubble = false;

        if (BubbleShielded)
            bubble = true;

        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (bubble)
            numberOfHits--;

        StartCoroutine(DamageHealingEffect(true, damage.ToString(), numberOfHits));
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        base.TakeHealing(healing, ignoreVigor);

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString()));
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        base.TakeMultiHitHealing(healing, numberOfHeals, ignoreVigor);

        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        StartCoroutine(DamageHealingEffect(false, healing.ToString(), numberOfHeals));
    }

    public override void Energize(bool energize)
    {
        //I am not figuring out how to make trash mobs work with this function
        if(!IsTrash)
            base.Energize(energize);
    }

    IEnumerator DamageHealingEffect(bool damage, string text = null, int numberOfHits = 1)
    {
        for(int i = 0; i < numberOfHits; i++)
        {
            damageTextStartPosition = damageText.transform.localPosition;
            healingTextStartPosition = healingText.transform.localPosition;

            //COLE TODO replace color with SFX
            if (damage)
            {
                damageText.text = text;

                foreach (SpriteRenderer renderer in TargetingSprites)
                    renderer.color = damageColor;

                damageText.color = damageColor;
                while (damageText.color.a > 0)
                {
                    damageText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                    damageText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);

                    foreach (SpriteRenderer renderer in TargetingSprites)
                        renderer.color += new Color(fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, 1);

                    yield return null;
                }
            }

            //COLE TODO replace color with SFX
            else if (!damage)
            {
                healingText.text = text;

                foreach (SpriteRenderer renderer in TargetingSprites)
                    renderer.color = healingColor;

                healingText.color = healingColor;
                while (healingText.color.a > 0)
                {
                    healingText.transform.position += new Vector3(moveSpeed * Time.deltaTime, moveSpeed * Time.deltaTime, 0);
                    healingText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);

                    foreach (SpriteRenderer renderer in TargetingSprites)
                        renderer.color += new Color(fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime, 1);

                    yield return null;
                }
            }

            damageText.transform.localPosition = damageTextStartPosition;
            healingText.transform.localPosition = healingTextStartPosition;
        }
    }

    public void QueueNextMove()
    {
        SpecialTarget(SelectAttack());
    }

    //Reserved for enemies with no special AI
    protected virtual int SelectAttack()
    {
        if (attacks.Length <= 0)
            return -1;

        CurrentTargets.Clear();
        ChosenAttack = attacks[UnityEngine.Random.Range(0, attacks.Length)];
        return GetAttackIndex();
    }

    protected virtual void SpecialTarget(int attackIndex) { }
}
