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
    public int CurrentStacks = 0;
    public Vector2 StackValue;

    [Header("Spawner")]
    public GameObject AUG;
    GameObject temp;

    public virtual void DebuffEffect() 
    {
        temp = Instantiate(AUG);
        temp.GetComponent<Augment>().Activate(this);
    }

    public virtual void StopEffect() 
    {
        temp.GetComponent<Augment>().StopEffect();
        Destroy(temp);
    }
}
