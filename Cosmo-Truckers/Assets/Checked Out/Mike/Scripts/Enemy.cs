using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;
    [SerializeField] int spaceTaken = 1;

    [SerializeField] Loot[] droppableLoot;
    [SerializeField] BaseAttackSO[] attacks;
    [SerializeField] List<DebuffStackSO> AUGS = new List<DebuffStackSO>();
    [SerializeField] EnemyPassiveBase passiveMove;
    public BaseAttackSO[] GetAllAttacks { get => attacks; }

    Animator enemyAnimation;
    EnemyManager enemyManager;
    SpriteRenderer myRenderer;
    TurnOrder turnOrder;
    int currentHealth;
    bool lootRolled = false;
    public int Health { get => currentHealth;  set => health = value; }
    public int GetSpaceTaken { get => spaceTaken; }

    private void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        myRenderer = GetComponent<SpriteRenderer>();
        turnOrder = FindObjectOfType<TurnOrder>();
        enemyAnimation = GetComponent<Animator>();
        currentHealth = health;
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
            passiveMove.Activate(currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddDebuffStack(DebuffStackSO stack)
    {
        foreach (var aug in AUGS)
        {
            if (String.Equals(aug.DebuffName, stack.DebuffName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks++;

                return;
            }
        }

        AUGS.Add(stack);
    }

    private void Die()
    {
        GetComponent<CharacterStats>().enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterStats>());
        myRenderer.enabled = false;
        
        foreach(Enemy enemy in enemyManager.Enemies)
        {
            if (enemy.name == gameObject.name)
            {
                enemyManager.Enemies.Remove(enemy);
                if(!lootRolled)
                {
                    lootRolled = true;
                    RollLoot();
                }
                break;
            }
        }
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

    public void Resurect(int newHealth)
    {
        currentHealth = newHealth;
        GetComponent<CharacterStats>().enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterStats>());
        myRenderer.enabled = true;
        turnOrder.DetermineTurnOrder();
    }

    public void StartTurn()
    {
        StartCoroutine(ProcessTurn());
    }

    public void EndTurn()
    {
        
    }

    IEnumerator ProcessTurn()
    {
        yield return new WaitForSeconds(2f);

        FindObjectOfType<CombatManager>().StartTurnEnemy(attacks[UnityEngine.Random.Range(0, attacks.Length)]);
    }

    public string GetName() { return characterName; }
}
