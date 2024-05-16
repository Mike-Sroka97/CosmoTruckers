using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTurnOrder : TurnOrder
{
    int turn = 0;
    AeglarCharacter aeglar;
    SafeTCharacter safeT;
    ProtoCharacter proto;
    SixFaceCharacter sixFace;
    List<Enemy> malites = new List<Enemy>();

    protected override void StartTurn()
    {
        if (!aeglar)
            SetCharacters();

        StartCoroutine(HandleTurnTutorial());
    }

    private void SetCharacters()
    {
        //Grab ya boys
        aeglar = FindObjectOfType<AeglarCharacter>();
        safeT = FindObjectOfType<SafeTCharacter>();
        proto = FindObjectOfType<ProtoCharacter>();
        sixFace = FindObjectOfType<SixFaceCharacter>();

        aeglar.GetManaBase.Tutorial = true;
        safeT.GetManaBase.Tutorial = true;
        proto.GetManaBase.Tutorial = true;
        sixFace.GetManaBase.Tutorial = true;

        //Grab ya enemies
        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            if (enemy.CharacterName == "Malite")
                malites.Add(enemy);
    }

    IEnumerator HandleTurnTutorial()
    {
        turn++;

        switch (turn)
        {
            case 1:
                //INA goes BLAH BLAH
                aeglar.GetManaBase.SetMaxMana();
                aeglar.GetManaBase.TutorialAttackName = "Porkanator";
                //Force Action
                aeglar.StartTurn();
                //INA listener to yap when attack wheel is open
                //INA listener to yap when attack is selected
                //INA listener to yap when attack starts (LOL)
                break;
            case 2:
                //INA yaps
                proto.GetManaBase.TutorialAttackName = "Electro-Whip";
                //Force Augment List
                //INA listener for Aug list open (TODO: reenable combat button)
                proto.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                //INA listener to yap when attack wheel is open
                //INA listener to yap when attack starts (LOL)
                break;
            case 3:
                //INA yaps about taunting from last turn at this point
                sixFace.GetManaBase.TutorialAttackName = "Petty Theft";
                //Force Insight
                //INA listener for Insight open (TODO: reenable combat button and aug list)
                sixFace.StartTurn();
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[0];
                //INA listener to yap when attack wheel is open
                //INA listener to yap when attack starts (LOL)
                break;
            case 4:
                safeT.GetManaBase.TutorialAttackName = "Clock Out";
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                safeT.StartTurn();
                //INA listener to yap when attack wheel is open
                //INA listener to yap when attack starts (LOL)
                break;
            case 5:
                malites[0].CurrentTargets.Clear();
                malites[0].CurrentTargets.Add(proto);
                malites[0].StartTurn();
                //INA listener to yap when attack starts (LOL)
                break;
            case 6:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(safeT);
                malites[1].StartTurn();
                break;
            case 7:
                //(TODO enable all buttons)
                aeglar.GetManaBase.TutorialAttackName = "Veggie Vengeance";
                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<ProtoCharacter>();
                aeglar.StartTurn();
                //INA listener to yap when attack wheel is open
                break;
            case 8:
                //(TODO enable all buttons)
                proto.GetManaBase.TutorialAttackName = "Spark Shield";
                proto.StartTurn();
                //INA listener to yap when attack wheel is open
                break;
            case 9:
                sixFace.GetManaBase.TutorialAttackName = "Bribery";
                sixFace.StartTurn();
                //INA listener to yap when attack wheel is open
                break;
            case 10:
                safeT.GetManaBase.SetMaxMana();
                safeT.GetManaBase.TutorialAttackName = "Power Pummel";
                CombatManager.Instance.MyTargeting.ForcedTarget = EnemyManager.Instance.Enemies[1];
                safeT.StartTurn();
                //INA listener to yap when attack wheel is open (TODO make the safeT.GetManaBase.SetMaxMana(); call here after INA brings attention to it)
                break;
            case 11:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(aeglar);
                malites[1].StartTurn();
                break;
            case 12:
                CombatManager.Instance.MyTargeting.ForcedTarget = FindObjectOfType<AeglarCharacter>();
                aeglar.StartTurn();
                break;
            case 13:
                proto.GetManaBase.TutorialAttackName = "Energon Jab";
                proto.StartTurn();
                break;
            case 14:
                //Malice malice's all over the place
                break;
            default:
                break;
        }

        yield return null;
    }
}
