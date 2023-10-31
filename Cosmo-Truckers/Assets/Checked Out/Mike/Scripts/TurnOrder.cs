using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    [HideInInspector] public static TurnOrder Instance;

    [SerializeField] TextMeshProUGUI endCombatText;
    [SerializeField] string victoryText = "Trucker Victory";
    [SerializeField] string lossText = "Trucker Loss";

    [SerializeField] List<CharacterStats> speedList;
    CharacterStats[] livingCharacters;
    int currentCharactersTurn = 0;
    bool combatOver = false;
    private void Start()
    {
        StartCoroutine(StartWait());
    }

    IEnumerator StartWait()
    {
        combatOver = false;
        //Small wait before combat starts
        yield return new WaitForSeconds(1.5f);

        speedList = new List<CharacterStats>();
        livingCharacters = FindObjectsOfType<CharacterStats>();
        foreach (CharacterStats speed in livingCharacters)
        {
            speedList.Add(speed);
        }

        DetermineTurnOrder();
        StartTurn();
    }

    public void DetermineTurnOrder()
    {        
        Array.Sort(livingCharacters, new SpeedComparer());

        foreach (CharacterStats speed in livingCharacters)
        {
            if (speed.GetComponent<PlayerCharacter>())
            {
                //Debug.Log(speed.GetComponent<PlayerCharacter>().GetName() + " " + speed.speed);
            }
            else
            {
                //Debug.Log(speed.GetComponent<Enemy>().GetName() + " " + speed.speed);
            }
        }
    }

    private void StartTurn()
    {
        //Determine if the combat is won/lost
        DetermineCombatEnd();

        if(!combatOver)
        {
            //give player control if player
            //give ai control if AI
            if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
            {
                livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().StartTurn();
            }
            else
            {
                //This is really ugly
                //Need to get if the enemy is trash and then get the first (boss trash) mob
                if (livingCharacters[currentCharactersTurn].GetComponent<Enemy>().isTrash && EnemyManager.Instance.TrashMobCollection[livingCharacters[currentCharactersTurn].GetComponent<Enemy>().CharacterName][0] == livingCharacters[currentCharactersTurn].GetComponent<Enemy>())
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
                //Mob is not trash and has independent turns
                else if(!livingCharacters[currentCharactersTurn].GetComponent<Enemy>().isTrash)
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
                else
                    EndTurn();
            }

            Debug.Log("It is " + livingCharacters[currentCharactersTurn].name + "'s turn");
        }
    }

    public void EndTurn()
    {
        if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
        {
            livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().EndTurn();
        }
        else
        {
            livingCharacters[currentCharactersTurn].GetComponent<Enemy>().EndTurn();
        }

        currentCharactersTurn++;
        if(currentCharactersTurn >= livingCharacters.Length)
        {
            currentCharactersTurn = 0;
        }
        StartTurn();
    }

    public bool AdjustSpeed(CharacterStats characterSpeed, bool increase)
    {
        //prevents the skipping of a characters turn
        if(increase)
        {
            if (currentCharactersTurn + 1 < livingCharacters.Length)
            {
                if (livingCharacters[currentCharactersTurn + 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
            else
            {
                if (livingCharacters[0].name == characterSpeed.name)
                {
                    return false;
                }
            }
        }
        else
        {
            if (currentCharactersTurn - 1 > 0)
            {
                if (livingCharacters[currentCharactersTurn - 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
            else
            {
                if (livingCharacters[livingCharacters.Length - 1].name == characterSpeed.name)
                {
                    return false;
                }
            }
        }

        foreach (CharacterStats speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                Debug.Log("here");
                speed.Reflex = characterSpeed.Reflex;
            }
        }
        return true;
    }

    public void RemoveFromSpeedList(CharacterStats characterSpeed)
    {
        foreach(CharacterStats speed in speedList)
        {
            if(speed.gameObject.name == characterSpeed.name)
            {
                speedList.Remove(speed);
                break;
            }
        }
        livingCharacters = speedList.ToArray();

        EnemyManager.Instance.FuckTrashMobs();
    }

    public void AddToSpeedList(CharacterStats characterSpeed)
    {
        foreach (CharacterStats speed in speedList)
        {
            if (speed.gameObject.name == characterSpeed.name)
            {
                return;
            }
        }
        speedList.Add(characterSpeed);
        livingCharacters = speedList.ToArray();

        EnemyManager.Instance.FuckTrashMobs();
    }

    public void EndCombat()
    {

    }

    private void DetermineCombatEnd()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
        {
            if (!enemy.Dead)
            {
                allEnemiesDead = false;
                break;
            }

        }

        if (allEnemiesDead)
        {
            //kill all enemy summons?
            endCombatText.text = victoryText;
            combatOver = true;
            return;
        }   

        foreach(PlayerCharacter character in EnemyManager.Instance.Players)
        {
            if (!character.Dead)
            {
                allPlayersDead = false;
                break;
            }
        }

        if(allPlayersDead)
        {
            //kill all player summons?
            endCombatText.text = lossText;
            combatOver = true;
            return;
        }
    }
}
