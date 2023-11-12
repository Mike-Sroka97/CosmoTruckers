using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SixFaceAttack", menuName = "ScriptableObjects/Attacks/SixFaceAttack")]

public class SixFaceAttackSO : BaseAttackSO
{
    public SixFaceMana.FaceTypes faceType;
    public bool RequiresMegaloManic = false;
}
