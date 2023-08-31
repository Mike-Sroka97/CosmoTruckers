using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Health;
    public int CurrentHealth;
    public bool Dead;

    protected TurnOrder turnOrder;

    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Dead = true;
        GetComponent<CharacterStats>().enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }

    public virtual void Resurrect(int newHealth)
    {
        CurrentHealth = newHealth;
        GetComponent<CharacterStats>().enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }

    public abstract void StartTurn();
    public abstract void EndTurn();
}
