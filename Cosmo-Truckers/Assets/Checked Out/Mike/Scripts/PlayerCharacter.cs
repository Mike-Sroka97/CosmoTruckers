using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;
    [SerializeField] GameObject wheel;
    [SerializeField] BaseAttackSO[] attacks; //For testing
    public BaseAttackSO[] GetAllAttacks { get => attacks; }

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
        if (currentHealth <= 0)
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

    public void StartTurn()
    {
        wheel.SetActive(true);
        wheel.GetComponentInChildren<AttackUI>().StartTurn(this);
    }
    public void EndTurn()
    {
        wheel.SetActive(false);
    }
    public string GetName() { return characterName; }
}
