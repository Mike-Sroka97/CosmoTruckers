using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsatiableHunger : CombatMove
{
    private void Start()
    {
        EndMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            foreach (Character character in CombatManager.Instance.GetCharactersSelected)
            {
                character.GetComponent<Character>().TakeDamage(Damage * Hits);

                if (DebuffToAdd != null)
                    for (int i = 0; i < Hits; i++)
                        character.GetComponent<Character>().AddDebuffStack(DebuffToAdd);
            }
        }
    }
}
