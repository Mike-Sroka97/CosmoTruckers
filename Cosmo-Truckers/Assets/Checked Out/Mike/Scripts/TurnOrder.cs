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
    protected CharacterStats[] livingCharacters;
    protected int currentCharactersTurn = 0;
    bool combatOver = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTurnOrder()
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
        StartCoroutine(DetermineCombatEnd());
    }

    public void DetermineTurnOrder()
    {       
        Array.Sort(livingCharacters, new SpeedComparer());
    }

    protected virtual void StartTurn()
    {
        if (!combatOver)
        {
            // Set each living character's combat stars layer to 0 at the beginning of a turn 
            foreach (CharacterStats character in livingCharacters)
                character.GetComponent<Character>().CombatStarsCurrentLayer = 0; 

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

        CombatManager.Instance.CurrentCharacter.EndTurn();

        CombatManager.Instance.CurrentCharacter.GetComponent<Character>().FadeAugments();

        //Handle characters with extra turns
        if (CombatManager.Instance.CurrentCharacter.Tireless)
        {
            livingCharacters[currentCharactersTurn].GetComponent<Character>().Energize(false);
            if(CombatManager.Instance.CurrentCharacter.Dead)
                currentCharactersTurn++;
        }
        else
            currentCharactersTurn++;

        if (currentCharactersTurn >= livingCharacters.Length)
            currentCharactersTurn = 0;

        StartCoroutine(DetermineCombatEnd());
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

        if (characterSpeed.GetComponent<EnemySummon>())
            EnemyManager.Instance.EnemySummons.Remove(characterSpeed.GetComponent<EnemySummon>());
        else if (characterSpeed.GetComponent<PlayerCharacterSummon>())
            EnemyManager.Instance.PlayerSummons.Remove(characterSpeed.GetComponent<PlayerCharacterSummon>());
    }

    public void AddToSpeedList(CharacterStats characterSpeed)
    {
        foreach (CharacterStats speed in speedList)
            if (speed.gameObject.name == characterSpeed.name)
                return;

        speedList.Add(characterSpeed);
        livingCharacters = speedList.ToArray();
        DetermineTurnOrder();

        for (int i = 0; i < livingCharacters.Length; i++)
            if (livingCharacters[i].GetComponent<Character>() == CombatManager.Instance.GetCurrentCharacter)
                currentCharactersTurn = i;
    }

    //Every thing for end combat not on a delay
    public void EndCombat()
    {
        HandleEOCaugments();
        StartCoroutine(EndCombatDelay());
    }

    private void HandleEOCaugments()
    {
        AugmentStackSO[] allAugments = FindObjectsOfType<AugmentStackSO>();

        //Remove all end of combat augs
        foreach (AugmentStackSO augment in allAugments)
        {
            if (augment.RemoveOnEndCombat)
                augment.MyCharacter.RemoveAugmentStack(augment);
        }
    }

    //everything for combat that will be on a delay
    IEnumerator EndCombatDelay()
    {
        //TODO Combat is not being set to true when loading into test and by passing the combat node selection
        if (CombatManager.Instance.InCombat)
        {
            yield return new WaitForSeconds(2.0f);

            //If testing the game in real play mode
            if (NetworkManager.singleton)
            {
                //TODO IF DUNGEONCONTROLLER.CURRENTNODE == BOSS
                //if (CombatData.Instance.lastNode)
                //{
                //    //Currently only dungeon map
                //    //Will need to change scene based on the last galaxy that the player was in
                //    //Also need to mark this current dungeon as compleated if nessisarry
                //    PlayerPrefs.SetInt("CurrentDungeon", PlayerPrefs.GetInt("CurrentDungeon", 0) + 1);
                //    NetworkManager.singleton.ServerChangeScene("GloomGloomGalaxyOW");
                //}
            }

            //Not using the network manager, will cause issues if we load in the dungeon so just reload this scene for now
            if (CombatData.Instance)
            {
                foreach (var character in FindObjectsOfType<PlayerCharacter>())
                {
                    EnemyManager.Instance.SavePlayerData(character);
                }

                CombatManager.Instance.EndCharacterCombatEffects();
                //No change scene
                //Bring up INA and redraw the dungeon
                combatOver = false;

                //TODO
                //Reset shield and Mana
                foreach (PlayerVessel character in PlayerVesselManager.Instance.PlayerVessels)
                {
                    character.MyCharacter.GetManaBase.ResetMana();
                    character.ManuallySetShield(0);
                }


                if(CombatManager.Instance.CurrentNode.NodeData.GetComponent<DungeonCombatNode>().Boss)
                {
                    //TODO do sicko mode post boss
                    StartCoroutine(CameraController.Instance.DungeonEnd(CombatManager.Instance.CurrentNode.NodeData.GetComponent<DungeonCombatNode>().SceneToLoad));
                    CameraController.Instance.transform.position = CombatManager.Instance.LastCameraPosition;
                    CameraController.Instance.Leader = CombatManager.Instance.DungeonCharacterInstance;
                }
                else
                {
                    StartCoroutine(FlipScreenWait());
                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            endCombatText.text = "";
        }
    }

    IEnumerator FlipScreenWait()
    {
        CombatManager.Instance.InCombat = false;

        //Start the flippy floppy as long as it is in scene
        FlipLoadAnimation flip = FindObjectOfType<FlipLoadAnimation>();
        if (flip)
        {
            flip.InitFlip();
            yield return new WaitUntil(() => !flip.IsFlipping);
        }

        CombatManager.Instance.CurrentNode.CombatDone = true;
        FindObjectOfType<DungeonController>().CurrentCombat++;
        CombatManager.Instance.CurrentNode.Active = true;
        CombatManager.Instance.CurrentNode.SetupLineRendererers();
    }

    private IEnumerator DetermineCombatEnd()
    {
        while (CombatManager.Instance.CommandsExecuting > 0)
        {
            yield return null; 
        }

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

        foreach (PlayerCharacter character in EnemyManager.Instance.Players)
        {
            if (!character.Dead)
            {
                allPlayersDead = false;
                break;
            }
        }

        if (allPlayersDead)
        {
            //kill all player summons?
            if (CombatManager.Instance.InCombat)
                endCombatText.text = lossText;

            combatOver = true;
        }
        else if (allEnemiesDead)
        {
            //kill all enemy summons?
            if (CombatManager.Instance.InCombat)
                endCombatText.text = victoryText;
            combatOver = true;
        }

        // Start the turn after determining if combat is over
        StartTurn(); 
    }
}
