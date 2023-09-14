using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public string CharacterName { get; private set; }

    [SerializeField] Loot[] droppableLoot;
    [SerializeField] BaseAttackSO[] attacks;

    public BaseAttackSO[] GetAllAttacks { get => attacks; }
    [HideInInspector] public PlayerCharacter TauntedBy;

    Animator enemyAnimation;
    EnemyManager enemyManager;
    SpriteRenderer myRenderer;
    bool lootRolled = false;

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

    private void RollLoot()
    {
        foreach(Loot loot in droppableLoot)
        {
            int randomResult = UnityEngine.Random.Range(1, 100);
            if(loot.GetDropChance() >= randomResult)
            {
                FindObjectOfType<LootManager>().AddLoot(loot);
            }
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
        //not needed yet but added for abstract
    }

    IEnumerator ProcessTurn()
    {
        FadeAugments();

        yield return new WaitForSeconds(2f);

        FindObjectOfType<CombatManager>().StartTurnEnemy(attacks[UnityEngine.Random.Range(0, attacks.Length)], this);
    }

    public override void AdjustDefense(int defense)
    {
        Stats.Defense += defense;

        if (Stats.Defense > 100)
            Stats.Defense = 100;
        else if (Stats.Defense < -200)
            Stats.Defense = -200;
    }
}
