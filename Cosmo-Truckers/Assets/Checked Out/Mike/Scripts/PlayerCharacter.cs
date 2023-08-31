using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] string characterName;
    [SerializeField] GameObject wheel;
    [SerializeField] List<BaseAttackSO> attacks;
    List<BaseAttackSO> attackClones = new List<BaseAttackSO>();
    [SerializeField] List<DebuffStackSO> AUGS = new List<DebuffStackSO>();

    public List<DebuffStackSO> GetAUGS { get => AUGS; }

    [Space(10)]
    [SerializeField] GameObject MiniGameControllerPrefab;
    public GameObject GetCharacterController { get => MiniGameControllerPrefab; }
    public List<BaseAttackSO> GetAllAttacks { get => attackClones; }

    private void Start()
    {
        foreach (var atk in attacks)
            attackClones.Add(Instantiate(atk));

        turnOrder = FindObjectOfType<TurnOrder>();
        CurrentHealth = Health;
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

    public override void StartTurn()
    {
        wheel.SetActive(true);
        wheel.GetComponentInChildren<AttackUI>().StartTurn(this);
    }
    public override void EndTurn()
    {
        wheel.SetActive(false);
    }
    public string GetName() { return characterName; }
}
