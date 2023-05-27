using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "ScriptableObjects/BaseAttack")]
public class BaseAttackSO : ScriptableObject
{
    public string AttackName;
    [SerializeField] public EnumManager.TargetingType targetingType;
    [SerializeField] public bool canTargetFriendly;
    [SerializeField] public bool canTargetEnemies;
    [SerializeField] public int numberOFTargets;
    [Space(10)]
    public GameObject CombatPrefab;
    public GameObject PlayerPrefab; //TEMP
    public float MiniGameTime;

    public virtual void StartCombat() { }
    public virtual void EndCombat() { }
}
