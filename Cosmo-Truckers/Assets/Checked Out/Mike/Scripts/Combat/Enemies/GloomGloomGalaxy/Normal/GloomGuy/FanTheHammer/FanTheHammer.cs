using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTheHammer : CombatMove
{
    [SerializeField] int startingScore;
    [SerializeField] float maxTime;

    const int numberOfBulletsInARevolver = 6;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        FanTheHammerGloomGuy[] gloomGuys = GetComponentsInChildren<FanTheHammerGloomGuy>();

        foreach (FanTheHammerGloomGuy gloomGuy in gloomGuys)
            gloomGuy.Initialize(this);


        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;
        if(PlayerDead)
        {
            Score = 1;
        }

        int damage = CalculateScore();

        DealMultiHitDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage, numberOfBulletsInARevolver);
    }
}
