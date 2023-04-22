using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "ScriptableObjects/BaseAttack")]
public class BaseAttackSO : ScriptableObject
{
    public string AttackName;
    [SerializeField] protected EnumManager.TargetingType targetingType;
    [SerializeField] protected bool canTargetFriendly;
    [SerializeField] protected bool canTargetEnemies;
    [SerializeField] protected int numberOFTargets;

    public virtual void StartCombat() { }
    public virtual void EndCombat() { }
}
