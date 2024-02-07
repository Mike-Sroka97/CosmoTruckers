using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        Instance = this;
    }

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
    }

    private void StartTurn()
    {
        //Determine if the combat is won/lost
        DetermineCombatEnd();

        if (!combatOver)
        {
            //give player control if player
            //give ai control if AI
            if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>())
            {
                if (livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().Stunned)
                {
                    livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().Stun(false);
                    livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().EndTurn();
                    CombatManager.Instance.HandleEndOfTurn();
                }
                else
                    livingCharacters[currentCharactersTurn].GetComponent<PlayerCharacter>().StartTurn();
            }
            else
            {
                //This is really ugly
                //Need to get if the enemy is trash and then get the first (boss trash) mob
                if(livingCharacters[currentCharactersTurn].GetComponent<Enemy>().IsTrash && livingCharacters[currentCharactersTurn].GetComponent<Enemy>().Stunned && EnemyManager.Instance.TrashMobCollection[livingCharacters[currentCharactersTurn].GetComponent<Enemy>().CharacterName].Count == 1)
                {
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().Stun(false);
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().EndTurn();
                    CombatManager.Instance.HandleEndOfTurn();
                }
                else if (livingCharacters[currentCharactersTurn].GetComponent<Enemy>().IsTrash && EnemyManager.Instance.TrashMobCollection[livingCharacters[currentCharactersTurn].GetComponent<Enemy>().CharacterName][0] == livingCharacters[currentCharactersTurn].GetComponent<Enemy>())
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
                //Mob is not trash and has independent turns
                else if (!livingCharacters[currentCharactersTurn].GetComponent<Enemy>().IsTrash)
                    livingCharacters[currentCharactersTurn].GetComponent<Enemy>().StartTurn();
                else
                    EndTurn();
            }

            Debug.Log("It is " + livingCharacters[currentCharactersTurn].name + "'s turn");
        }
        //Combat is over
        else
            EndCombat();
    }

    public void EndTurn()
    {
        EnemyManager.Instance.UpdateTrashMobList();

        livingCharacters[currentCharactersTurn].GetComponent<Character>().FadeAugments();

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
        DetermineTurnOrder();

        //EnemyManager.Instance.UpdateTrashMobList();
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
        DetermineTurnOrder();

        //EnemyManager.Instance.UpdateTrashMobList();
    }

    //Every thing for end combat not on a delay
    public void EndCombat()
    {
        StartCoroutine(EndCombatDelay());
    }
    //everything for combat that will be on a delay
    IEnumerator EndCombatDelay()
    {
        //TODO replace with loot screen and other end combat options before moving forward to dungeon screen
        yield return new WaitForSeconds(2.0f);

        //If testing the game in real play mode
        if (NetworkManager.singleton)
        {
            if (CombatData.Instance.lastNode)
            {
                //Currently only dungeon map
                //TODO
                //Will need to change scene based on the last galaxy that the player was in
                //Also need to mark this current dungeon as compleated if nessisarry
                PlayerPrefs.SetInt("CurrentDungeon", PlayerPrefs.GetInt("CurrentDungeon", 0) + 1);
                NetworkManager.singleton.ServerChangeScene("GloomGloomGalaxyOW");
            }
            else
            {
                foreach (var character in FindObjectsOfType<PlayerCharacter>())
                {
                    EnemyManager.Instance.SavePlayerData(character);
                }
                NetworkManager.singleton.ServerChangeScene("DungeonSelection");
            }
        }
        //Not using the network manager, will cause issues if we load in the dungeon so just reload this scene for now
        else if (CombatData.Instance)
        {
            if (CombatData.Instance.lastNode)
            { 
                //Currently only dungeon map
                //TODO
                //Will need to change scene based on the last galaxy that the player was in
                //Also need to mark this current dungeon as compleated if nessisarry
                PlayerPrefs.SetInt("CurrentDungeon", PlayerPrefs.GetInt("CurrentDungeon", 0) + 1);
                SceneManager.LoadScene("GloomGloomGalaxyOW");
            }
            else
            {
                foreach (var character in FindObjectsOfType<PlayerCharacter>())
                {
                    EnemyManager.Instance.SavePlayerData(character);
                }
                SceneManager.LoadScene("DungeonSelection");
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
