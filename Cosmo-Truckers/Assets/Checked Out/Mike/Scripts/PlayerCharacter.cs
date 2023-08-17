using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int health;
    [SerializeField] int currentHealth;
    [SerializeField] GameObject wheel;
    [SerializeField] List<BaseAttackSO> attacks;
    List<BaseAttackSO> attackClones = new List<BaseAttackSO>();
    [SerializeField] List<DebuffStackSO> AUGS = new List<DebuffStackSO>();

    public List<DebuffStackSO> GetAUGS { get => AUGS; }

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    TurnOrder turnOrder;
    public bool Dead { get; private set; }

    private void Start()
    {
        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

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

    public void AddDebuffStack(DebuffStackSO stack)
    {
        foreach(var aug in AUGS)
        {
            if(String.Equals(aug.DebuffName, stack.DebuffName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks++;

                return;
            }
        }

        AUGS.Add(Instantiate(stack));

        if (stack.Type == DebuffStackSO.ActivateType.StartUp)
            stack.DebuffEffect();
    }

    private void Die()
    {
        Dead = true;
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
