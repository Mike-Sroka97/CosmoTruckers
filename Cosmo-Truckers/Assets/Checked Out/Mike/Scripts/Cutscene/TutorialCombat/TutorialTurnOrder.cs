using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTurnOrder : TurnOrder
{
    int turn = 0;

    protected override void StartTurn()
    {
        turn++;
        StartCoroutine(HandleTurnTutorial());

        //give player control if player
        //give ai control if AI
        if (turn == 14)
            return;

        else if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
            livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().StartTurn();

        else
            livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
    }

    IEnumerator HandleTurnTutorial()
    {
        switch (turn)
        {
            case 1:
                //Goofy ah text COLE
                //while(tutorial text yield return null
                //set aeglar porked
                //start aeglar turn
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            default:
                break;
        }

        yield return null;
    }
}
