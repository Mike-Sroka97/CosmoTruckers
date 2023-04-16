using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterINA", menuName = "ScriptableObjects/PlayerCharacterINA")]
public abstract class PlayerCharacterINA : ScriptableObject
{
    public virtual void Movement() { }
    public virtual void Attack() { }
    public virtual void SpecialMove() { }
    public virtual void Jump() { }
}
