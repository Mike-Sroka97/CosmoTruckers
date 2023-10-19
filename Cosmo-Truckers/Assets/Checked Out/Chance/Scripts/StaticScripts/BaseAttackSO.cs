using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "ScriptableObjects/Attacks/BaseAttack")]
public class BaseAttackSO : ScriptableObject
{
    public bool CanUse = true;
    [Space(10)]

    public string AttackName;
    [SerializeField] public EnumManager.TargetingType TargetingType;
    [SerializeField] public bool canTargetFriendly;
    [SerializeField] public bool friendlyPositiveEffect;
    [SerializeField] public bool canTargetEnemies;
    [SerializeField] public bool enemyPositiveEffect;
    [SerializeField] public int NumberOFTargets;
    [Space(10)]
    public GameObject CombatPrefab;
    public float MiniGameTime;

    public virtual void StartCombat() { }
    public virtual void EndCombat() { }
}
