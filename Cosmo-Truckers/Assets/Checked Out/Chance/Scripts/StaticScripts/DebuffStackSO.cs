using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all debuffs
[CreateAssetMenu(fileName = "Debuff", menuName = "ScriptableObjects/Debuff/EmptyBase")]
public class DebuffStackSO : ScriptableObject
{
    [Header("Base Variables")]
    public string DebuffName;
    [TextArea(5, 10)]public string DebuffDescription;

    [Header("Stacks")]
    public bool Stackable = false;
    public int MaxStacks = 10;
    public Vector2 StackValue;

    public virtual void DebuffEffect() { Debug.Log($"{DebuffName} activated"); }
}
