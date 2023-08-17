using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPassiveBase: MonoBehaviour
{
    [System.Serializable]
    public enum PassiveType
    {
        OnDamage,
        OnStartMiniGame,
        OnStartBattle,
    }

    [SerializeField] PassiveType type;
    public PassiveType GetPassiveType { get => type; }

    public virtual void Activate() { }
    public virtual void Activate(int val) { }
}
