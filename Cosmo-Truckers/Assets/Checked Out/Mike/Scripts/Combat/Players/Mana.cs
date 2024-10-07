using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mana : MonoBehaviour
{
    [Header("Debug Fields")]
    [SerializeField] protected bool freeSpells = false; 

    protected PlayerCharacter myCharacter;
    protected List<BaseAttackSO> attacks;
    [HideInInspector] public PlayerVessel MyVessel;
    [HideInInspector] public bool Tutorial;
    [HideInInspector] public string TutorialAttackName;


    private void Start()
    {
        myCharacter = GetComponent<PlayerCharacter>();
        attacks = myCharacter.GetAllAttacks;
    }

    public virtual void SetVessel(PlayerVessel newVessel)
    {
        MyVessel = newVessel;
    }

    public void TutorialCheckCastableSpells()
    {
        foreach (BaseAttackSO attack in attacks)
            if (attack.AttackName != TutorialAttackName)
                attack.CanUse = false;
            else
                attack.CanUse = true;
    }

    public abstract void CheckCastableSpells();

    public abstract void SetMaxMana();

    public abstract void ResetMana();
}
