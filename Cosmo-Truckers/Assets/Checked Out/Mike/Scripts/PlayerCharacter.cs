using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;

    int currentHealth;

    private void Start()
    {
        currentHealth = health;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<CharacterSpeed>().enabled = false;
        FindObjectOfType<TurnOrder>().RemoveFromSpeedList(GetComponent<CharacterSpeed>());
        FindObjectOfType<TurnOrder>().DetermineTurnOrder();
    }
    public string GetName() { return characterName; }
}
