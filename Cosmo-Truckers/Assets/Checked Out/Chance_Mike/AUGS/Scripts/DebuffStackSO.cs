using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all debuffs
[CreateAssetMenu(fileName = "Debuff", menuName = "ScriptableObjects/Debuff/EmptyBase")]
public class DebuffStackSO : ScriptableObject
{
    public enum ActivateType
    {
        InCombat,
        StartUp,
        TurnStart,
        TurnEnd,
        StatChange,
        OnDamage
    }

    [Header("Base Variables")]
    public string DebuffName;
    public ActivateType Type = ActivateType.InCombat;
    [TextArea(5, 10)]public string DebuffDescription;
    [HideInInspector] public Character MyCharacter;
    public bool IsBuff;
    public bool IsDebuff;

    [Header("Stacks")]
    public bool Stackable = false;
    public int MaxStacks = 10;
    public int CurrentStacks = 0;
    [Header("X = Initial Stack Value, Y = Subsequent Stack Values")]
    public Vector2 StackValue;
    public bool FadingAugment { get; protected set; }
    [SerializeField] int fadePerTurn = 0;

    [Header("Spawner")]
    public GameObject AugSpawner;
    GameObject temp;

    public virtual void DebuffEffect() 
    {
            temp = Instantiate(AugSpawner);
            temp.GetComponent<Augment>().Activate(this);
    }

    public virtual void StopEffect() 
    {
            temp.GetComponent<Augment>().StopEffect();
            Destroy(temp);
    }

    public Augment GetAugment()
    {
        return temp.GetComponent<Augment>();
    }

    public void Fade()
    {
        //If the stacks are at or less than 0 then remove the GO from scene and stop the effect
        if(temp.GetComponent<Augment>().GetStacks <= 0)
        {
            StopEffect();
            return;
        }

        temp.GetComponent<Augment>().AdjustStatusEffect(fadePerTurn);
    }
}
