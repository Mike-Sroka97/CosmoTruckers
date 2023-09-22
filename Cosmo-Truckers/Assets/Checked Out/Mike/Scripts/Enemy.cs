using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public string CharacterName { get; private set; }

    [SerializeField] protected BaseAttackSO[] attacks;

    public BaseAttackSO[] GetAllAttacks { get => attacks; }
    [HideInInspector] public PlayerCharacter TauntedBy;
    public bool SpecialTargetConditions = false;
    protected BaseAttackSO ChosenAttack;

    Animator enemyAnimation;
    EnemyManager enemyManager;
    SpriteRenderer myRenderer;

    private void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        myRenderer = GetComponent<SpriteRenderer>();
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
        enemyAnimation.enabled = true;
    }

    public void EndTarget()
    {
        enemyAnimation.enabled = false;
    }

    public override void Die()
    {
        base.Die();
        
        //Think about our lives
        //foreach(Enemy enemy in enemyManager.Enemies)
        //{
        //    if (enemy.name == gameObject.name)
        //    {
        //        enemyManager.Enemies.Remove(enemy);
        //        if(!lootRolled)
        //        {
        //            lootRolled = true;
        //            RollLoot();
        //        }
        //        break;
        //    }
        //}
        if(enemyManager.Enemies.Count <= 0)
        {
            turnOrder.EndCombat();
        }
        else
        {
            turnOrder.DetermineTurnOrder();
        }
    }

    public override void Resurrect(int newHealth)
    {
        base.Resurrect(newHealth);
        myRenderer.enabled = true;
        //if an enemy is in this spot do not
    }

    public override void StartTurn()
    {
        StartCoroutine(ProcessTurn());
    }

    public override void EndTurn()
    {
        TauntedBy = null;
    }

    IEnumerator ProcessTurn()
    {
        FadeAugments();

        yield return new WaitForSeconds(2f);

        //AI is dumb and does not have target cons
        if (ChosenAttack == null)
            ChosenAttack = attacks[UnityEngine.Random.Range(0, attacks.Length)];

        FindObjectOfType<CombatManager>().StartTurnEnemy(ChosenAttack, this);
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

    protected virtual void SpecialTarget(int attackIndex) { }
}
