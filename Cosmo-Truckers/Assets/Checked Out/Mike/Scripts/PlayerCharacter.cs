using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;
    [SerializeField] Canvas myButtons;
    [SerializeField] Attack[] attacks;

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
        //foreach (GameObject child in myButtons.GetComponentsInChildren<GameObject>())
        //{
        //    child.SetActive(true);
        //}
    }
    public void EndTurn()
    {
        //foreach (GameObject child in myButtons.GetComponentsInChildren<GameObject>())
        //{
        //    child.SetActive(false);
        //}
    }
    public string GetName() { return characterName; }
}
