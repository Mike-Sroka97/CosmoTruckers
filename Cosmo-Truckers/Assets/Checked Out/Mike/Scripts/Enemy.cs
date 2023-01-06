using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;

    TurnOrder turnOrder;
    int currentHealth;

    private void Start()
    {
        turnOrder = FindObjectOfType<TurnOrder>();
        currentHealth = health;
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
        turnOrder.DetermineTurnOrder();
    }

    public void Resurect(int newHealth)
    {
        currentHealth = newHealth;
        GetComponent<CharacterSpeed>().enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterSpeed>());
        turnOrder.DetermineTurnOrder();
    }
    public string GetName() { return characterName; }
}
