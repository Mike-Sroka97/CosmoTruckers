using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;
    [SerializeField] Loot[] droppableLoot;

    Animator enemyAnimation;
    EnemyManager enemyManager;
    SpriteRenderer myRenderer;
    TurnOrder turnOrder;
    int currentHealth;
    bool lootRolled = false;

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        myRenderer = GetComponent<SpriteRenderer>();
        turnOrder = FindObjectOfType<TurnOrder>();
        enemyAnimation = FindObjectOfType<Animator>();
        currentHealth = health;
    }

    //TODO this is being stupid and I have no idea why
    public void StartTarget()
    {
        enemyAnimation.enabled = true;
        print(gameObject.name + "ON");
    }

    public void EndTarget()
    {
        enemyAnimation.enabled = false;
        print(gameObject.name + "Off");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<CharacterSpeed>().enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterSpeed>());
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
        GetComponent<CharacterSpeed>().enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterSpeed>());
        myRenderer.enabled = true;
        turnOrder.DetermineTurnOrder();
    }

    public void StartTurn()
    {
        StartCoroutine(ProcessTurn());
    }

    IEnumerator ProcessTurn()
    {
        yield return new WaitForSeconds(2f);
        turnOrder.EndTurn();
    }
    public string GetName() { return characterName; }
}
