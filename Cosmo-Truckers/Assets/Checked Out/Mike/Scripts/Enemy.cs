using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] float fadeSpeed;
    [SerializeField] float moveSpeed;
    Vector3 damageTextStartPosition;
    Vector3 healingTextStartPosition;

    public BaseAttackSO[] GetAllAttacks { get => attacks; }
    [HideInInspector] public PlayerCharacter TauntedBy;
    public bool SpecialTargetConditions = false;
    protected BaseAttackSO ChosenAttack;

    [Header("Trash mob collector")]
    public bool IsTrash = false;
    [SerializeField] string characterName;

    Animator enemyAnimation;

    private void Awake()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        turnOrder = FindObjectOfType<TurnOrder>();
        enemyAnimation = GetComponent<Animator>();
        CurrentHealth = Health;
    }

    private void Start()
    {
        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnStartBattle)
            StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        passiveMove.Activate(this);
    }

    public void StartTarget()
    {

    }

    public void EndTarget()
    {

    }

    public override void Resurrect(int newHealth)
    {
        base.Resurrect(newHealth);
        myRenderer.enabled = true;
        //if an enemy is in this spot do not
    }

    public override void StartTurn()
    {
        FadeAugments();

        if (ChosenAttack == null)
            ChosenAttack = attacks[UnityEngine.Random.Range(0, attacks.Length)];

        CombatManager.Instance.StartTurnEnemy(ChosenAttack, this);
    }

    public override void EndTurn()
    {
        TauntedBy = null;
    }

    public override void AdjustDefense(int defense)
    {
        Stats.Defense += defense;

        if (Stats.Defense > 100)
            Stats.Defense = 100;
        else if (Stats.Defense < -200)
            Stats.Defense = -200;
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

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        base.TakeDamage(damage, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        StartCoroutine(DamageHealingEffect(true, damage.ToString()));
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        StartCoroutine(DamageHealingEffect(true, damage.ToString(), numberOfHits));
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if(!ignoreVigor)
            base.TakeHealing(healing);

        if(healing != 0)
            StartCoroutine(DamageHealingEffect(false, healing.ToString()));
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


    protected virtual void SpecialTarget(int attackIndex) { }
}
