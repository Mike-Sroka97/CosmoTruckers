using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mana : MonoBehaviour
{
    protected PlayerCharacter myCharacter;
    protected List<BaseAttackSO> attacks;
    public PlayerVessel MyVessel;

    private void Start()
    {
        myCharacter = GetComponent<PlayerCharacter>();
        attacks = myCharacter.GetAllAttacks;
    }

    public virtual void SetVessel(PlayerVessel newVessel)
    {
        MyVessel = newVessel;
    }

    public abstract void CheckCastableSpells();
}
