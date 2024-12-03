using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockingShock : CombatMove
{
    [SerializeField] float lightningDelay;
    [SerializeField] float twoScoreTime = 12f;
    [SerializeField] float maxScoreTime = 15f;

    ShockingShockLightning[] lightning;
    int numberOfLightningToAssign;
    [HideInInspector] public int CurrentActivatedLightning = 0;
    float lightningTime = 0;

    private void Start()
    {
        GenerateLayout();
        lightning = GetComponentsInChildren<ShockingShockLightning>();
        numberOfLightningToAssign = lightning.Length - 1; //always activate all lightning minus one
    }

    public override void StartMove()
    {
        trackTime = true;
        GetComponentInChildren<PlayerBasedParabolaMovement>().SetPlayer(FindObjectOfType<Player>());
        GetComponentInChildren<PlayerBasedParabolaMovement>().Active = true;

        base.StartMove();
    }

    private void Update()
    {
        CheckShockStatus();
        TrackTime();
    }

    private void CheckShockStatus()
    {
        if(CurrentActivatedLightning > 0)
        {
            trackTime = false;
        }
        else
        {
            trackTime = true;
        }
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        base.TrackTime();

        lightningTime += Time.deltaTime;
        
        if(currentTime >= lightningDelay)
        {
            lightningTime = 0;
            HandleLightning();
        }
    }

    private void HandleLightning()
    {
        foreach(ShockingShockLightning shock in lightning)
        {
            if(shock.AlwaysActivate)
            {
                shock.ActivateMe();
                shock.IsShocking = true;
            }
        }

        while(CurrentActivatedLightning < numberOfLightningToAssign)
        {
            int random = UnityEngine.Random.Range(0, lightning.Length);
            if(!lightning[random].IsShocking)
            {
                lightning[random].ActivateMe();
                lightning[random].IsShocking = true;
            }
        }
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        //Get needed info
        PlayerCharacter player = null;
        Enemy enemy = null;
        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            if (character.GetComponent<Enemy>())
                enemy = character.GetComponent<Enemy>();
            else if (character.GetComponent<PlayerCharacter>())
                player = character.GetComponent<PlayerCharacter>();
        }

        int stacksToRemove = 0;
        foreach(AugmentStackSO aug in player.GetAUGS)
        {
            if (aug.AugmentName == "Shocked")
                stacksToRemove = aug.CurrentStacks;
        }

        //Run scores
        MoveEnded = true;

        int timeScore = (int)currentTime;

        if (timeScore >= maxScoreTime)
            Score = 0;
        else if (timeScore >= twoScoreTime)
            Score = 1;
        else
            Score = 2;

        int damage = CalculateScore();

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);

        //Remove enemy augments
        if (enemy)
            enemy.RemoveAmountOfAugments(stacksToRemove, 0);
    }
}
