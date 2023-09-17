using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [HideInInspector] public static CombatManager Instance;

    [SerializeField] GameObject TempPage;
    [SerializeField] GameObject MiniGameScreen;
    GameObject miniGame;
    GameObject character;   //Currently only one 
                            //Needs to be made into list for enemy multi target skills
    [SerializeField] TMP_Text Timer;

    [SerializeField] List<Character> CharactersSelected;

    public List<Character> GetCharactersSelected { get => CharactersSelected; }

    bool StartTimer = false;

    PlayerCharacter CurrentPlayer;
    Enemy CurrentEnemy;
    public PlayerCharacter GetCurrentPlayer { get => CurrentPlayer; }
    public Enemy GetCurrentEnemy { get => CurrentEnemy; }

    private void Awake() => Instance = this;

    public void StartCombat(BaseAttackSO attack, PlayerCharacter currentPlayer)
    {
        CharactersSelected.Clear();
        StartTimer = false;

        CurrentPlayer = currentPlayer;

        switch (attack.TargetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                TempPage.SetActive(true);
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                TempPage.SetActive(true);
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
                        TempPage.SetActive(true);
                        StartTimer = true;
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName} against {CharactersSelected[0].name}. . .");
                    });
                }
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                TempPage.SetActive(true);
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
                            TempPage.SetActive(true);
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
                TempPage.SetActive(true);
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
                        TempPage.SetActive(true);
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

        StartCoroutine(StartMiniGame(attack));
    }

    public void StartTurnEnemy(BaseAttackSO attack, Enemy enemy)
    {
        CurrentEnemy = enemy;
        StartCoroutine(EnemyDelay(attack, enemy));
    }

    IEnumerator EnemyDelay(BaseAttackSO attack, Enemy enemy)
    {
        PlayerCharacter[] attackable = FindObjectsOfType<PlayerCharacter>();
        CharactersSelected.Clear();
        CurrentPlayer = null;
        StartTimer = false;

        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        switch (attack.TargetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                TempPage.SetActive(true);
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                TempPage.SetActive(true);
                StartTimer = true;
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, self target. . .");
                break;
            #endregion
            #region Single Target
            case EnumManager.TargetingType.Single_Target:
                //enemy is taunted
                if(enemy.TauntedBy != null && !enemy.TauntedBy.Dead)
                {
                    if (CheckPlayerSummonLayer(enemy.TauntedBy.CombatSpot[0]))
                    {
                        CharactersSelected.Add(EnemyManager.Instance.PlayerSummons[enemy.TauntedBy.CombatSpot[0]]);
                    }
                    else
                    {
                        CharactersSelected.Add(enemy.TauntedBy);
                        TempPage.SetActive(true);
                        StartTimer = true;

                        character = Instantiate(enemy.TauntedBy.GetCharacterController);
                        character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * enemy.TauntedBy.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {CharactersSelected[0].name}. . .");
                        break;
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
                            if(CheckPlayerSummonLayer(obj.CombatSpot[0]))
                            {
                                CharactersSelected.Add(EnemyManager.Instance.PlayerSummons[obj.CombatSpot[0]]);
                            }
                            else
                            {
                                CharactersSelected.Add(obj);
                                TempPage.SetActive(true);
                                StartTimer = true;

                                character = Instantiate(obj.GetCharacterController);
                                character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {CharactersSelected[0].name}. . .");
                                break;
                            }
                        }
                    }
                }
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                TempPage.SetActive(true);
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
                    character = Instantiate(enemy.TauntedBy.GetCharacterController);
                    character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * enemy.TauntedBy.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                }

                foreach (var obj in attackable)
                {
                    if (!obj.Dead && !CharactersSelected.Contains(obj))
                    {
                        CharactersSelected.Add(obj);
                        character = Instantiate(obj.GetCharacterController);
                        character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                    }
                    if (CharactersSelected.Count == attack.NumberOFTargets)
                    {
                        TempPage.SetActive(true);
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
                        character = Instantiate(obj.GetCharacterController);
                        character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                        CharactersSelected.Add(obj);
                        TempPage.SetActive(true);
                        StartTimer = true;
                    }
                }
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, AOE. . .");
                break;
            #endregion
            #region All Target
            case EnumManager.TargetingType.All_Target:
                foreach (var obj in attackable)
                {
                    if (!obj.Dead)
                    {
                        character = Instantiate(obj.GetCharacterController);
                        character.GetComponent<Player>().MoveSpeed += character.GetComponent<Player>().MoveSpeed * obj.GetComponent<CharacterStats>().Speed * .01f; //adjusts speed
                        CharactersSelected.Add(obj);
                        TempPage.SetActive(true);
                        StartTimer = true;
                    }
                }

                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against all alive players. . .");
                break;
            #endregion

            default: Debug.LogError($"{attack.TargetingType} not set up."); EndCombat(); yield break;
        }

        StartCoroutine(StartMiniGame(attack));
    }

    IEnumerator StartMiniGame(BaseAttackSO attack)
    {
        float miniGameTime = attack.MiniGameTime;
        Timer.text = miniGameTime.ToString();

        miniGame = Instantiate(attack.CombatPrefab);
        miniGame.transform.SetParent(MiniGameScreen.transform);

        //Attack SO Start
        attack.StartCombat();

        while (!StartTimer) yield return null;

        if (CharactersSelected.Count > 0)
        {
            foreach (Character character in CharactersSelected) //PlayerCharacter invalid cast
            {
                foreach (var aug in character.GetAUGS)
                    if (aug.Type == DebuffStackSO.ActivateType.InCombat)
                        aug.DebuffEffect();
            }

            if (CurrentPlayer)
            {
                foreach (var aug in CurrentPlayer.GetAUGS)
                    if (aug.Type == DebuffStackSO.ActivateType.InCombat)
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
                    if (aug.Type != DebuffStackSO.ActivateType.OnDamage)
                        aug.StopEffect();
                }
            }
        }

        EndCombat();
    }

    public void EndCombat()
    {
        StopAllCoroutines();

        miniGame.GetComponentInChildren<CombatMove>().EndMove();

        TempPage.SetActive(false);
        CharactersSelected.Clear();
        Destroy(miniGame);
        Destroy(character);

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

    private bool CheckPlayerSummonLayer(int playerSpot)
    {
        if(EnemyManager.Instance.PlayerSummons[playerSpot] != null && !EnemyManager.Instance.PlayerSummons[playerSpot].Dead)
            return true;
        return false;
    }
}
