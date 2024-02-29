using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTCharacter : PlayerCharacter
{
    [SerializeField] DebuffStackSO strength;

    public override void StartCombatEffect()
    {
        EnemyManager.Instance.SummonSpawnedEvent.AddListener(AddCharacterToListener);

        foreach (Character character in FindObjectsOfType<Character>())
            character.DieEvent.AddListener(HandleStrengthGainPassive);
    }

    public override void EndCombatEffect()
    {
        EnemyManager.Instance.SummonSpawnedEvent.RemoveListener(AddCharacterToListener);

        foreach (Character character in FindObjectsOfType<Character>())
            character.DieEvent.RemoveListener(HandleStrengthGainPassive);
    }

    private void AddCharacterToListener()
    {
        Character[] characters = FindObjectsOfType<Character>();

        foreach (Character character in characters)
            character.DieEvent.RemoveListener(HandleStrengthGainPassive);

        foreach (Character character in characters)
            character.DieEvent.AddListener(HandleStrengthGainPassive);
    }

    private void HandleStrengthGainPassive()
    {
        if (CombatManager.Instance.GetCurrentCharacter.GetComponent<SafeTCharacter>())
            AddDebuffStack(strength);
    }
}
