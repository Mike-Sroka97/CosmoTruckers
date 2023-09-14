using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "ScriptableObjects/BaseAttack")]
public class BaseAttackSO : ScriptableObject
{
    public bool CanUse = true;
    [Space(10)]

    public string AttackName;
    [SerializeField] public EnumManager.TargetingType TargetingType;
    [SerializeField] public bool canTargetFriendly;
    [SerializeField] public bool canTargetEnemies;
    [SerializeField] public int NumberOFTargets;
    [Space(10)]
    public GameObject CombatPrefab;
    public GameObject PlayerPrefab; //TEMP
    public float MiniGameTime;

    public virtual void StartCombat() { }
    public virtual void EndCombat() { }
}
