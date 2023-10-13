using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mana : MonoBehaviour
{
    protected PlayerCharacter myCharacter;
    protected List<BaseAttackSO> attacks;

    private void Start()
    {
        myCharacter = GetComponent<PlayerCharacter>();
        attacks = myCharacter.GetAllAttacks;
    }

    public abstract void CheckCastableSpells();
}
