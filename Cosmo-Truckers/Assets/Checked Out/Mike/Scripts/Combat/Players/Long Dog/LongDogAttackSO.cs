using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LongDogAttack", menuName = "ScriptableObjects/Attacks/LongDogAttack")]

public class LongDogAttackSO : BaseAttackSO
{
    public bool RequiresHead;
    public bool RequiresBody;
    public bool RequiresLeg;

    public int RequiredBullets;
}
