using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPillPlacebo : CombatMove
{
    [HideInInspector] public Vector3 CurrentCheckPointLocation;

    SixFaceMana sixFaceMana;

    public override List<Character> NoTargetTargeting()
    {
        sixFaceMana = FindObjectOfType<SixFaceMana>();

        if(sixFaceMana.FaceType == SixFaceMana.FaceTypes.Dizzy)
        {
            return null;
        }
        else if(sixFaceMana.FaceType == SixFaceMana.FaceTypes.Money)
        {
            CombatManager.Instance.MyTargeting.CurrentTargetingType = EnumManager.TargetingType.Single_Target;
            CombatManager.Instance.MyTargeting.InitialSetup = false;
        }
        else
        {
            //Actual target condition
            PlayerCharacter lowestHealthPlayer = null;

            foreach(PlayerCharacter player in EnemyManager.Instance.Players)
            {
                if (!player.Dead && lowestHealthPlayer == null)
                    lowestHealthPlayer = player;
                else if (!player.Dead && player.CurrentHealth < lowestHealthPlayer.CurrentHealth)
                    lowestHealthPlayer = player;
            }

            List<Character> target = new List<Character>();
            target.Add(lowestHealthPlayer);
            return target;
        }

        return null;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        //if Dizzy do the dizzy thing
        sixFaceMana = FindObjectOfType<SixFaceMana>();
        if (sixFaceMana.FaceType == SixFaceMana.FaceTypes.Dizzy)
        {
            List<Character> alivePlayers = new List<Character>();
            foreach (PlayerCharacter player in EnemyManager.Instance.Players)
            {
                if (!player.Dead)
                    alivePlayers.Add(player);
            }

            int random = Random.Range(0, alivePlayers.Count);
            CombatManager.Instance.GetCharactersSelected.Add(alivePlayers[random]);
        }


        //set face
        sixFaceMana.UpdateFace();

        //minigame result
        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            //Calculate Healing
            if (Score < 0)
                Score = 0;
            if (Score >= maxScore)
                Score = maxScore;

            int currentHealing = 0;
            currentHealing = Score * Damage;
            currentHealing += baseDamage;

            //Calculate Augment Stacks
            int augmentStacks = 1; //always applies placebo

            //1 being base damage
            float HealingAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

            character.TakeHealing((int)(currentHealing * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), pierces);

            //Apply augment
            character.GetComponent<Character>().AddAugmentStack(DebuffToAdd, augmentStacks);
        }
    }
}
