using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [HideInInspector] public static CombatManager Instance;
    [SerializeField] private Sprite blankBG;
    public Sprite currentBG;

    [SerializeField] GameObject MiniGameScreen;
    GameObject miniGame;
    public GameObject GetMiniGame { get => miniGame; }
    List<GameObject> characters;   //Currently only one 
                            //Needs to be made into list for enemy multi target skills
    [SerializeField] TMP_Text Timer;
    public Targeting MyTargeting;
    [SerializeField] float enemySpellHoldTime;
    [SerializeField] float trashAttackDelay = .25f;

    [SerializeField] public List<Character> CharactersSelected;
    public List<PlayerCharacter> ActivePlayers;

    public List<Character> GetCharactersSelected { get => CharactersSelected; }

    PlayerCharacter CurrentPlayer;
    List<PlayerCharacter> attackable;
    Enemy CurrentEnemy;
    Character CurrentCharacter;
    public PlayerCharacter GetCurrentPlayer { get => CurrentPlayer; }

    public BaseAttackSO CurrentAttack;
    public Enemy GetCurrentEnemy { get => CurrentEnemy; }

    public Character GetCurrentCharacter { get => CurrentCharacter; }

    public bool INAmoving = false;

    bool inTrashEndMove = false;

    private void Awake() => Instance = this;
    [HideInInspector] public bool TargetsSelected = true;

    public void StartCombat(BaseAttackSO attack, PlayerCharacter currentPlayer)
    {
        CurrentAttack = attack;
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        //To ensure this is cleared on player turn
        CurrentEnemy = null;
        CurrentCharacter = currentPlayer;
        CurrentPlayer = currentPlayer;
        ActivePlayers.Add(currentPlayer);
        TargetsSelected = false;
        MyTargeting.StartTargeting(attack);

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    public void StartTurnEnemy(BaseAttackSO attack, Enemy enemy)
    {
        CurrentAttack = attack;
        CurrentEnemy = enemy;
        EnemyTarget(attack, enemy);
    }

    private void EnemyTarget(BaseAttackSO attack, Enemy enemy)
    {
        attackable = new List<PlayerCharacter>();
        foreach (PlayerCharacter character in EnemyManager.Instance.PlayerCombatSpots)
            if (character != null)
                attackable.Add(character);
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        CurrentPlayer = null;
        CurrentCharacter = enemy;

        if(enemy.SpecialTargetConditions)
        {
            enemy.TargetConditions(attack);

            List<PlayerCharacter> charactersToSpawn = new List<PlayerCharacter>();

            foreach (Character character in CharactersSelected)
                if (character.GetComponent<PlayerCharacter>())
                    ActivePlayers.Add(character.GetComponent<PlayerCharacter>());
        }
        else
        {
            switch (attack.TargetingType)
            {
                #region No Target
                case EnumManager.TargetingType.No_Target:
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                    break;
                #endregion
                #region Self Target
                case EnumManager.TargetingType.Self_Target:
                    CharactersSelected.Add(enemy);
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, self target. . .");
                    break;
                #endregion
                #region Single Target
                case EnumManager.TargetingType.Single_Target:
                    SingleTargetEnemy(attack, enemy);
                    break;
                #endregion
                #region Multi Target Cone
                case EnumManager.TargetingType.Multi_Target_Cone:
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .");
                    break;
                #endregion
                #region Multi Target Choice
                case EnumManager.TargetingType.Multi_Target_Choice:
                    System.Random multiRand = new System.Random();
                    PlayerCharacter[] attackableCharacters = attackable.OrderBy(x => multiRand.Next()).ToArray();

                    //taunted
                    if (enemy.TauntedBy != null && !CharactersSelected.Contains(enemy.TauntedBy))
                    {
                        CharactersSelected.Add(enemy.TauntedBy);
                        ActivePlayers.Add(enemy.TauntedBy);
                    }

                    foreach (var obj in attackable)
                    {
                        if (!obj.Dead && !CharactersSelected.Contains(obj))
                        {
                            CharactersSelected.Add(obj);
                            ActivePlayers.Add(obj);
                        }
                        if (CharactersSelected.Count == attack.NumberOfTargets)
                        {
                            string text = $"Doing Combat Stuff for {attack.AttackName} against";
                            for (int i = 0; i < CharactersSelected.Count; i++)
                                text += $" { CharactersSelected[i].name } & ";

                            text.Remove(text.Length - 2, 2);
                            text += ". . .";
                            Debug.Log(text);
                            break;
                        }
                    }
                    break;
                #endregion
                #region AOE
                case EnumManager.TargetingType.AOE:
                    foreach (var obj in attackable)
                    {
                        if (!obj.Dead)
                        {
                            CharactersSelected.Add(obj);
                            ActivePlayers.Add(obj);
                        }
                    }
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, AOE. . .");
                    break;
                #endregion
                #region All Target
                case EnumManager.TargetingType.All_Target:
                    AllTargetEnemy(attack);
                    break;
                #endregion
            }
        }

        MyTargeting.EnemyTargeting(attack, enemySpellHoldTime);

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    public void SingleTargetEnemy(BaseAttackSO attack, Enemy enemy)
    {
        //enemy is taunted
        if (enemy.TauntedBy != null && !enemy.TauntedBy.Dead)
        {
            if (CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
            {
                CharactersSelected.Add(EnemyManager.Instance.PlayerCombatSpots[enemy.TauntedBy.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
            }
            else
            {
                CharactersSelected.Add(enemy.TauntedBy);
                ActivePlayers.Add(enemy.TauntedBy);
            }
        }
        //enemy is not taunted
        else
        {
            System.Random singleRand = new System.Random();
            PlayerCharacter[] attackableCharacters = attackable.OrderBy(x => singleRand.Next()).ToArray();
            foreach (PlayerCharacter obj in attackable)
            {
                if (!obj.Dead)
                {
                    if (!obj.GetComponent<PlayerCharacterSummon>() && CheckPlayerSummonLayer(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]))
                    {
                        CharactersSelected.Add(EnemyManager.Instance.PlayerCombatSpots[obj.CombatSpot + EnemyManager.Instance.playerSummonIndexAdder]);
                        //TODO CHECK IF COMBAT SPOT IS OF TYPE PLAYERCHARACTERSUMMON THEN ADD SUMMONER REFERENCE TO ACTIVEPLAYERS
                        ActivePlayers.Add(obj);

                        break;
                    }
                    else
                    {
                        CharactersSelected.Add(obj);
                        ActivePlayers.Add(obj);

                        break;
                    }
                }
            }
        }
    }

    public void AllTargetEnemy(BaseAttackSO attack)
    {
        foreach (PlayerCharacter obj in attackable)
        {
            if (!obj.Dead)
            {
                //character = Instantiate(obj.GetCharacterController);
                //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                CharactersSelected.Add(obj);
                ActivePlayers.Add(obj);
            }
        }

        Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against all alive players. . .");
    }

    IEnumerator StartMiniGame(BaseAttackSO attack, List<PlayerCharacter> charactersToSpawn)
    {
        while (!TargetsSelected)
            yield return null;

        INAcombat INA = MiniGameScreen.GetComponentInParent<INAcombat>();
        float miniGameTime = attack.MiniGameTime;
        Timer.text = miniGameTime.ToString();

        INAmoving = true;
        miniGame = Instantiate(attack.CombatPrefab, INA.transform);
        StartCoroutine(INA.MoveINA(true));

        while (INAmoving)
            yield return null;

        MiniGameScreen.GetComponent<SpriteRenderer>().sprite = currentBG;

        //Attack SO Start
        foreach (Player player in FindObjectsOfType<Player>())
        {
            player.enabled = true;
        }

        attack.StartCombat();

        if (ActivePlayers.Count > 0)
        {
            foreach (PlayerCharacter character in ActivePlayers) //PlayerCharacter invalid cast
            {
                for(int i = 0; i < character.GetAUGS.Count; i++)
                {
                    if (character.GetAUGS[i].InCombat)
                        character.GetAUGS[i].DebuffEffect();
                }
                //foreach (var aug in character.GetAUGS)
                //    if (aug.InCombat)
                //        aug.DebuffEffect();
            }
        }

        while (miniGameTime >= 0 && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
        {
            miniGameTime -= Time.deltaTime;
            Timer.text = ((int)miniGameTime).ToString();

            yield return null;
        }


        StopAllCoroutines();

        INAmoving = true;

        StartCoroutine(INA.MoveINA(false));
    }

    public void SpawnPlayers()
    {
        foreach (PlayerCharacter player in ActivePlayers)
        {
            GameObject character = Instantiate(player.GetCharacterController);
            characters.Add(character);
            character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * player.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
            character.GetComponent<Player>().enabled = false;
        }
    }

    public IEnumerator EndCombat()
    {
        //If this is the only place combat end move is called this should work, if it gets called from multiple sources it may cause some strange issues
        if(CurrentEnemy != null && CurrentEnemy.IsTrash)
        {
            StartCoroutine(DelayEndMove());
            while(inTrashEndMove)
            {
                yield return null;
            }
        }
        else
        {
            miniGame.GetComponentInChildren<CombatMove>().EndMove();
        }

        miniGame.gameObject.SetActive(true);
        Destroy(miniGame);
        CharactersSelected.Clear();

        //Handle EoT augments (store this data and display visuals in EndCombat()?
        DebuffStackSO[] allAugments = FindObjectsOfType<DebuffStackSO>();

        foreach (DebuffStackSO augment in allAugments)
        {
            if (augment.EveryTurnEnd)
                augment.StopEffect();
        }

        Augment[] augments = FindObjectsOfType<Augment>();

        foreach (Augment augment in augments)
        {
            Destroy(augment.gameObject);
        }

        foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
        {
            obj.EndTarget();
        }

        FindObjectOfType<TurnOrder>().EndTurn();
    }

    IEnumerator DelayEndMove()
    {
        inTrashEndMove = true;

        //Loop over all the trash enemys
        foreach (var enemy in EnemyManager.Instance.TrashMobCollection)
        {
            if (enemy.Key == CurrentEnemy.CharacterName)
            {
                //Found the enemy type and now loop over them all to deal damage independently
                for (int i = 0; i < enemy.Value.Count; i++)
                {
                    CurrentEnemy = enemy.Value[i];

                    miniGame.GetComponentInChildren<CombatMove>().EndMove();
                    yield return new WaitForSeconds(trashAttackDelay);
                }
            }
        }

        inTrashEndMove = false;
    }

    public void CleanupMinigame()
    {
        //Clean up INA
        foreach (GameObject character in characters)
        {
            Destroy(character);
        }

        miniGame.gameObject.SetActive(false);
    }

    private void OnDisable() => Instance = null;

    public bool CheckPlayerSummonLayer(Character summon)
    {
        if(summon != null && !summon.Dead)
            return true;
        return false;
    }
}
