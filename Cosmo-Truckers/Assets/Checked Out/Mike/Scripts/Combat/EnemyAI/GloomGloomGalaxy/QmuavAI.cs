using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QmuavAI : Enemy
{
    int lastAttack = -1;
    int random = 0;
    public int NumberOfTimesHitLastRound = 0;
    bool castBossMove = false;
    bool shiddedAndFardded = false;
    GalasterHordeAI myGalaster;

    private void Start()
    {
        myGalaster = FindObjectOfType<GalasterHordeAI>();
        myGalaster.transform.position = transform.position;

        //Add targeting sprites
        List<SpriteRenderer> tempSpriteList = new List<SpriteRenderer>();

        //Non shield
        foreach (SpriteRenderer spriteRenderer in TargetingSprites)
            tempSpriteList.Add(spriteRenderer);
        foreach (SpriteRenderer spriteRenderer in myGalaster.TargetingSprites)
            tempSpriteList.Add(spriteRenderer);
        TargetingSprites = tempSpriteList.ToArray();

        //Shield
        tempSpriteList.Clear();
        foreach (SpriteRenderer spriteRenderer in ShieldSprites)
            tempSpriteList.Add(spriteRenderer);
        foreach (SpriteRenderer spriteRenderer in myGalaster.ShieldSprites)
            tempSpriteList.Add(spriteRenderer);
        ShieldSprites = tempSpriteList.ToArray();
    }

    public override void StartTurn()
    {
        //Cast Final Spell
        if (castBossMove)
            random = 11;
        //Shid and Fard
        else if(myGalaster.Dead && !shiddedAndFardded)
        {
            shiddedAndFardded = true;
            random = 9;
        }
        //Rez galaster ball
        else if(myGalaster.Dead && shiddedAndFardded)
        {
            shiddedAndFardded = false;
            random = 10;
        }
        //Prevents the same spell from being cast twice in a row
        else if (lastAttack != -1)
        {
            while (lastAttack == random)
                random = Random.Range(0, attacks.Length - 3); //minus 1 to avoid casting non random spells
        }
        else
            random = Random.Range(0, attacks.Length - 3); //minus 1 to avoid casting non random spells

        //Basic turn start stuff
        ChosenAttack = attacks[random];
        lastAttack = random;
        base.StartTurn();
    }

    protected override void SpecialTarget(int attackIndex)
    {
        //Harm and Repulse
        if (attackIndex == 0)
        {
            //Random single target
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Hive Healing
        else if (attackIndex == 1)
        {
            //Add Qmuav
            CombatManager.Instance.GetCharactersSelected.Add(this);

            //Add Support or Random
            if(CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Twisted Targeting
        else if (attackIndex == 2)
        {
            //AOE targeting
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
        //Atomic Impact
        else if (attackIndex == 3)
        {
            //Find DPS or random
            if (CombatManager.Instance.FindDPSCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindDPSCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Gravitic Siphon
        else if (attackIndex == 4)
        {
            //Random single target
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);

            //Add Qmuav
            CombatManager.Instance.GetCharactersSelected.Add(this);
        }
        //Galaxy Burst
        else if (attackIndex == 5)
        {
            //Find Utility or random
            if (CombatManager.Instance.FindUtilityCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindUtilityCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Task Master
        else if (attackIndex == 6)
        {
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
            CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Gloom Guarded
        else if (attackIndex == 7)
        {
            //Add Qmuav
            CombatManager.Instance.GetCharactersSelected.Add(this);

            //Add Support or Random
            if (CombatManager.Instance.FindSupportCharacter())
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this, CombatManager.Instance.FindSupportCharacter());
            else
                CombatManager.Instance.SingleTargetEnemy(ChosenAttack, this);
        }
        //Supermassive Amplifier
        else if (attackIndex == 8)
        {
            //AOE targeting
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
        //Cry Baby
        else if (attackIndex == 9)
        {
            //Just shidding and fardding
        }
        //Galaster Recall
        else if (attackIndex == 10)
        {
            //Just rezzing galaster ball
        }
        //Cry of Frustration (BOSS MOVE)
        else if (attackIndex == 11)
        {
            //AOE targeting
            CombatManager.Instance.AOETargetPlayers(ChosenAttack);
        }
    }

    public override void EndTurn()
    {
        //Resets hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound = 0;
        base.EndTurn();
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        //Adds single hit to hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound++;

        //Galaster horde will jump in front of the president
        if (myGalaster.Dead)
            base.TakeDamage(damage, defensePiercing);
        else
            myGalaster.TakeDamage(damage, defensePiercing);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        //Adds each hit to hit counter (used for gravitic siphon)
        NumberOfTimesHitLastRound += numberOfHits;

        //Galaster horde will jump in front of the president
        if (myGalaster.Dead)
            base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);
        else
            myGalaster.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);
    }

    public override void Die()
    {
        castBossMove = true;
    }

    public void DieForReal()
    {
        base.Die();
    }
}
