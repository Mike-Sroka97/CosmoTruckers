using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "ScriptableObjects/Attacks/BaseAttack")]
public class BaseAttackSO : ScriptableObject
{
    public bool CanUse = true;
    [Space(10)]

    public string AttackName;
    public EnumManager.TargetingType TargetingType;
    public bool CanTargetFriendly;
    public bool FriendlyPositiveEffect;
    public bool CanTargetEnemies;
    public bool EnemyPositiveEffect;
    public int NumberOfTargets;
    public bool TargetsDead;
    public bool AutoCast;
    public bool BossMove;
    [Space(10)]
    public GameObject CombatPrefab;
    public float MiniGameTime;
    [TextArea(15, 20)]
    public string AttackDescription;
    public VideoClip MinigameDemo;

    public virtual void StartCombat() { }
    public virtual void EndCombat() { }
}
