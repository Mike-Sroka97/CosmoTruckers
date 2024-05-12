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
                //Goofy ah text COLE
                //while(tutorial text yield return null
                aeglar.GetManaBase.SetMaxMana();
                aeglar.GetManaBase.TutorialAttackName = "Porkanator";
                aeglar.StartTurn();
                break;
            case 2:
                proto.GetManaBase.TutorialAttackName = "Electro-Whip";
                proto.StartTurn();
                //Force target malite 0
                break;
            case 3:
                safeT.GetManaBase.TutorialAttackName = "Clock Out";
                safeT.StartTurn();
                break;
            case 4:
                sixFace.GetManaBase.TutorialAttackName = "Petty Theft";
                sixFace.StartTurn();
                break;
            case 5:
                malites[0].CurrentTargets.Clear();
                malites[0].CurrentTargets.Add(proto);
                malites[0].StartTurn();
                break;
            case 6:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(safeT);
                malites[1].StartTurn();
                break;
            case 7:
                aeglar.GetManaBase.TutorialAttackName = "Veggie Vengeance";
                aeglar.StartTurn();
                break;
            case 8:
                proto.GetManaBase.TutorialAttackName = "Spark Shield";
                proto.StartTurn();
                break;
            case 9:
                sixFace.GetManaBase.TutorialAttackName = "Bribery";
                sixFace.StartTurn();
                break;
            case 10:
                safeT.GetManaBase.SetMaxMana();
                safeT.GetManaBase.TutorialAttackName = "Power Pummel";
                safeT.StartTurn();
                break;
            case 11:
                malites[1].CurrentTargets.Clear();
                malites[1].CurrentTargets.Add(aeglar);
                malites[1].StartTurn();
                break;
            case 12:
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
