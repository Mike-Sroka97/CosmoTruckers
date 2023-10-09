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
    List<GameObject> characters;   //Currently only one 
                            //Needs to be made into list for enemy multi target skills
    [SerializeField] TMP_Text Timer;

    [SerializeField] public List<Character> CharactersSelected;
    private List<PlayerCharacter> ActivePlayers;

    public List<Character> GetCharactersSelected { get => CharactersSelected; }

    bool StartTimer = false;

    PlayerCharacter CurrentPlayer;
    PlayerCharacter[] attackable;
    Enemy CurrentEnemy;
    public PlayerCharacter GetCurrentPlayer { get => CurrentPlayer; }
    public Enemy GetCurrentEnemy { get => CurrentEnemy; }

    public bool INAmoving = false;

    private void Awake() => Instance = this;

    public void StartCombat(BaseAttackSO attack, PlayerCharacter currentPlayer)
    {
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        StartTimer = false;

        CurrentPlayer = currentPlayer;
        ActivePlayers.Add(currentPlayer);

        switch (attack.TargetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, self target. . .");
                break;
            #endregion
            #region Single Target
            case EnumManager.TargetingType.Single_Target:
                foreach (Enemy obj in FindObjectOfType<EnemyManager>().Enemies)
                {
                    obj.StartTarget();
                    Button button = obj.gameObject.GetComponentInChildren<Button>();
                    button.interactable = true;
                    button.onClick.AddListener(delegate
                    {
                        print(obj.gameObject.name);
                        CharactersSelected.Add(obj);
                        StartTimer = true;
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName} against {CharactersSelected[0].name}. . .");
                    });
                }
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .");
                break;
            #endregion
            #region Multi Target Choice
            case EnumManager.TargetingType.Multi_Target_Choice:
                foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
                {
                    obj.StartTarget();
                    Button button = obj.gameObject.GetComponentInChildren<Button>();
                    button.interactable = true;
                    button.onClick.AddListener(delegate
                    {
                        print(obj.gameObject.name);
                        button.interactable = false;
                        CharactersSelected.Add(obj);
                        if (CharactersSelected.Count == attack.NumberOFTargets || CharactersSelected.Count == FindObjectOfType<EnemyManager>().Enemies.Count)
                        {
                            StartTimer = true;
                            string text = $"Doing Combat Stuff for {attack.AttackName} against";
                            for (int i = 0; i < CharactersSelected.Count; i++)
                                text += $" { CharactersSelected[i].name } & ";

                            text.Remove(text.Length - 2, 2);
                            text += ". . .";
                            Debug.Log(text);
                        }
                    });
                }
                break;
            #endregion
            #region AOE
            case EnumManager.TargetingType.AOE:
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, AOE. . .");
                break;
            #endregion
            #region All Target
            case EnumManager.TargetingType.All_Target:
                foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
                {
                    CharactersSelected.Add(obj);
                    if (CharactersSelected.Count == FindObjectOfType<EnemyManager>().Enemies.Count)
                    {
                        StartTimer = true;
                        string text = $"Doing Combat Stuff for {attack.AttackName} against";
                        for (int i = 0; i < CharactersSelected.Count; i++)
                            text += $" { CharactersSelected[i].name } & ";

                        text.Remove(text.Length - 2, 2);
                        text += ". . .";
                        Debug.Log(text);
                    }
                }
                break;
            #endregion

            default: Debug.LogError($"{attack.TargetingType} not set up."); EndCombat(); return;
        }

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    public void StartTurnEnemy(BaseAttackSO attack, Enemy enemy)
    {
        CurrentEnemy = enemy;
        StartCoroutine(EnemyDelay(attack, enemy));
    }

    IEnumerator EnemyDelay(BaseAttackSO attack, Enemy enemy)
    {
        attackable = FindObjectsOfType<PlayerCharacter>();
        CharactersSelected.Clear();
        ActivePlayers = new List<PlayerCharacter>();
        characters = new List<GameObject>();
        CurrentPlayer = null;
        StartTimer = false;

        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        if(enemy.SpecialTargetConditions)
        {
            enemy.TargetConditions(attack);
            StartTimer = true;

            List<PlayerCharacter> charactersToSpawn = new List<PlayerCharacter>();

            foreach (Character character in CharactersSelected)
                if (character.GetComponent<PlayerCharacter>())
                    ActivePlayers.Add(character.GetComponent<PlayerCharacter>());

            //foreach(PlayerCharacter playerCharacter in charactersToSpawn)
            //{
            //    ActivePlayers.Add(playerCharacter);
            //    //character = Instantiate(playerCharacter.GetCharacterController);
            //    //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * playerCharacter.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
            //    //Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {CharactersSelected[0].name}. . .");
            //}
        }
        else
        {
            switch (attack.TargetingType)
            {
                #region No Target
                case EnumManager.TargetingType.No_Target:
                    StartTimer = true;
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                    break;
                #endregion
                #region Self Target
                case EnumManager.TargetingType.Self_Target:
                    StartTimer = true;
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
                    StartTimer = true;
                    Debug.Log($"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .");
                    break;
                #endregion
                #region Multi Target Choice
                case EnumManager.TargetingType.Multi_Target_Choice:
                    System.Random multiRand = new System.Random();
                    attackable = attackable.OrderBy(x => multiRand.Next()).ToArray();

                    //taunted
                    if (enemy.TauntedBy != null && !CharactersSelected.Contains(enemy.TauntedBy))
                    {
                        CharactersSelected.Add(enemy.TauntedBy);
                        ActivePlayers.Add(enemy.TauntedBy);
                        //character = Instantiate(enemy.TauntedBy.GetCharacterController);
                        //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * enemy.TauntedBy.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                    }

                    foreach (var obj in attackable)
                    {
                        if (!obj.Dead && !CharactersSelected.Contains(obj))
                        {
                            CharactersSelected.Add(obj);
                            ActivePlayers.Add(obj);
                            //character = Instantiate(obj.GetCharacterController);
                            //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                        }
                        if (CharactersSelected.Count == attack.NumberOFTargets)
                        {
                            StartTimer = true;
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
                            //character = Instantiate(obj.GetCharacterController);
                            //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                            CharactersSelected.Add(obj);
                            ActivePlayers.Add(obj);
                            StartTimer = true;
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

                default: Debug.LogError($"{attack.TargetingType} not set up."); EndCombat(); yield break;
            }
        }

        StartCoroutine(StartMiniGame(attack, ActivePlayers));
    }

    public void SingleTargetEnemy(BaseAttackSO attack, Enemy enemy)
    {
        //enemy is taunted
        if (enemy.TauntedBy != null && !enemy.TauntedBy.Dead)
        {
            if (CheckPlayerSummonLayer(enemy.TauntedBy.CombatSpot[0]))
            {
                CharactersSelected.Add(EnemyManager.Instance.PlayerSummons[enemy.TauntedBy.CombatSpot[0]]);
            }
            else
            {
                CharactersSelected.Add(enemy.TauntedBy);
                StartTimer = true;

                ActivePlayers.Add(enemy.TauntedBy);
                //character = Instantiate(enemy.TauntedBy.GetCharacterController);
                //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * enemy.TauntedBy.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                //Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {CharactersSelected[0].name}. . .");
            }
        }
        //enemy is not taunted
        else
        {
            System.Random singleRand = new System.Random();
            attackable = attackable.OrderBy(x => singleRand.Next()).ToArray();
            foreach (PlayerCharacter obj in attackable)
            {
                if (!obj.Dead)
                {
                    if (CheckPlayerSummonLayer(obj.CombatSpot[0]))
                    {
                        CharactersSelected.Add(EnemyManager.Instance.PlayerSummons[obj.CombatSpot[0]]);
                    }
                    else
                    {
                        CharactersSelected.Add(obj);
                        ActivePlayers.Add(obj);
                        StartTimer = true;

                        //character = Instantiate(obj.GetCharacterController);
                        //character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                        //Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {CharactersSelected[0].name}. . .");
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
                StartTimer = true;
            }
        }

        Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against all alive players. . .");
    }

    IEnumerator StartMiniGame(BaseAttackSO attack, List<PlayerCharacter> charactersToSpawn)
    {
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

        while (!StartTimer) yield return null;

        if (ActivePlayers.Count > 0)
        {
            foreach (PlayerCharacter character in ActivePlayers) //PlayerCharacter invalid cast
            {
                foreach (var aug in character.GetAUGS)
                    if (aug.InCombat)
                        aug.DebuffEffect();
            }
        }

        while (miniGameTime >= 0 && !miniGame.GetComponentInChildren<CombatMove>().PlayerDead && !miniGame.GetComponentInChildren<CombatMove>().MoveEnded)
        {
            if (StartTimer)
            {
                miniGameTime -= Time.deltaTime;
                Timer.text = ((int)miniGameTime).ToString();
            }

            yield return null;
        }


        if (CharactersSelected.Count > 0)
        {
            foreach (Character player in CharactersSelected)  //PlayerCharacter invalid cast
            {
                foreach (DebuffStackSO aug in player.GetAUGS)
                {
                    if (aug.OnDamage == false)
                        aug.StopEffect();
                }
            }
        }
        StopAllCoroutines();

        INAmoving = true;

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

    public void EndCombat()
    {
        miniGame.GetComponentInChildren<CombatMove>().EndMove();

        //delay



        //Clean up INA
        CharactersSelected.Clear();
        Destroy(miniGame);
        foreach(GameObject character in characters)
        {
            Destroy(character);
        }

        foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
        {
            obj.EndTarget();
            Button button = obj.gameObject.GetComponentInChildren<Button>();
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }



        FindObjectOfType<TurnOrder>().EndTurn();
    }

    private void OnDisable() => Instance = null;

    public bool CheckPlayerSummonLayer(int playerSpot)
    {
        if(EnemyManager.Instance.PlayerSummons.Count != 0 && EnemyManager.Instance.PlayerSummons[playerSpot] != null && !EnemyManager.Instance.PlayerSummons[playerSpot].Dead)
            return true;
        return false;
    }
}
