using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Base class for all debuffs
[CreateAssetMenu(fileName = "Debuff", menuName = "ScriptableObjects/Debuff/EmptyBase")]
[System.Serializable]
public class DebuffStackSO : ScriptableObject
{
    [Header("Types")]
    public bool InCombat;
    public bool StartUp;
    public bool TurnStart;
    public bool TurnEnd;
    public bool StatChange;
    public bool OnDamage;
    public bool EveryTurnEnd;
    public bool EveryTurnStart;
    public bool OnSpellCast;

    [Header("Base Variables")]
    public string DebuffName;
    [TextArea(5, 10)]public string DebuffDescription;
    [HideInInspector] public Character MyCharacter;
    public bool IsBuff;
    public bool IsDebuff;
    public bool Removable = true;
    public bool RemoveOnDeath = true;
    public bool RemoveOnEndCombat = false;

    [Header("Stacks")]
    public bool Stackable = false;
    public int MaxStacks = 10;

    [Header("UI")]
    public Sprite AugmentSprite;

    public int CurrentStacks
    {
        get
        {
            return currentStacks;
        }
        set
        {
            if (!initialized)
                initialized = true;
            else
                LastStacks = currentStacks;

            currentStacks = value;
            if (currentStacks > MaxStacks)
                currentStacks = MaxStacks;
        }
    }

    private bool initialized = false;
    public int LastStacks = -1;

    [SerializeField] private int currentStacks;

    [Header("X = Initial Stack Value, Y = Subsequent Stack Values")]
    public Vector2 StackValue;
    [SerializeField] int fadePerTurn = 0;

    [Header("Spawner")]
    public GameObject AugSpawner;
    GameObject temp;

    public void SetTemp()
    {
        if (temp == null)
        {
            temp = Instantiate(AugSpawner, MyCharacter.transform);
            temp.GetComponent<Augment>().InitializeAugment(this);
        }
    }

    public virtual void DebuffEffect() 
    {
        SetTemp();
        temp.GetComponent<Augment>().Activate(this);
    }

    public Augment GetAugment()
    {
        if (!temp)
            return null;

        return temp.GetComponent<Augment>();
    }

    public void Fade()
    {
        if (temp == null)
            return;

        //Display fade
        if (fadePerTurn > 0)
            MyCharacter.CallDisplayAugment(this, true);

        temp.GetComponent<Augment>().AdjustStatusEffect(-fadePerTurn);

        DestroyAugment();
    }

    public void DestroyAugment()
    {
        //If the stacks are at or less than 0 then remove the GO from scene and stop the effect
        if (CurrentStacks <= 0)
        {
            MyCharacter.AugmentsToRemove.Add(this);
            temp.GetComponent<Augment>().StopEffect();
            Destroy(temp);
        }
    }

    public void SetFade(int newFade) { fadePerTurn = newFade; }

    public int GetFade() { return fadePerTurn; }
}
