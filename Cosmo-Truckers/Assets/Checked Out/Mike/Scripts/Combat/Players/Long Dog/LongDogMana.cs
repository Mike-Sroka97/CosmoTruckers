using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogMana : Mana
{
    public bool HeadAlive = true;
    public bool BodyAlive = true;
    public bool LegAlive = true;

    const int clipSize = 5;
    public List<int> loadedBullets = new List<int>();
    public List<int> reserveBullets = new List<int>();

    public override void CheckCastableSpells()
    {
        if (freeSpells)
        {
            foreach (LongDogAttackSO attack in attacks)
            {
                attack.CanUse = true;
            }
        }
        else
        {
            foreach (LongDogAttackSO attack in attacks)
            {
                //Checks bullets
                if (loadedBullets.Count >= attack.RequiredBullets)
                    attack.CanUse = true;
                else
                {
                    attack.CanUse = false;
                    continue;
                }

                //Checks head (lol)
                if (attack.RequiresHead && HeadAlive)
                    attack.CanUse = true;
                else if(attack.RequiresHead)
                {
                    attack.CanUse = false;
                    continue;
                }

                //Checks body
                if (attack.RequiresBody && BodyAlive)
                    attack.CanUse = true;
                else if (attack.RequiresBody)
                {
                    attack.CanUse = false;
                    continue;
                }

                //Checks leg
                if (attack.RequiresLeg && LegAlive)
                    attack.CanUse = true;
                else if (attack.RequiresLeg)
                {
                    attack.CanUse = false;
                    continue;
                }

                attack.CanUse = true;
            }
        }
    }
}
