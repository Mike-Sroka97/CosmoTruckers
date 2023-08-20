using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [SerializeField] GameObject TempPage;
    [SerializeField] GameObject MiniGameScreen;
    GameObject miniGame;
    GameObject character;   //Currently only one 
                            //Needs to be made into list for enemy multi target skills
    [SerializeField] TMP_Text Timer;
 
    [SerializeField] List<GameObject> EnemySelected;
    [SerializeField] List<GameObject> PlayerSelected;

    public List<GameObject> GetPlayerSelected { get => PlayerSelected; }
    public List<GameObject> GetEnemySelected { get => EnemySelected; }

    bool StartTimer = false;

    PlayerCharacter CurrentPlayer;

    private void Awake() => Instance = this;

    public void StartCombat(BaseAttackSO attack, PlayerCharacter currentPlayer)
    {
        PlayerSelected.Clear();
        EnemySelected.Clear();
        StartTimer = false;

        CurrentPlayer = currentPlayer;

        switch (attack.targetingType)
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
                foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
                {
                    obj.StartTarget();
                    Button button = obj.gameObject.GetComponentInChildren<Button>();
                    button.interactable = true;
                    button.onClick.AddListener(delegate
                    {
                        print(obj.gameObject.name);
                        EnemySelected.Add(obj.gameObject);
                        TempPage.SetActive(true);
                        StartTimer = true;
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName} against {EnemySelected[0].name}. . .");
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
                        EnemySelected.Add(obj.gameObject);
                        if (EnemySelected.Count == attack.numberOFTargets || EnemySelected.Count == FindObjectOfType<EnemyManager>().Enemies.Count)
                        {
                            TempPage.SetActive(true);
                            StartTimer = true;
                            string text = $"Doing Combat Stuff for {attack.AttackName} against";
                            for(int i = 0; i < EnemySelected.Count; i++)
                                text += $" { EnemySelected[i].name } & ";

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
                    EnemySelected.Add(obj.gameObject);
                    if (EnemySelected.Count == FindObjectOfType<EnemyManager>().Enemies.Count)
                    {
                        TempPage.SetActive(true);
                        StartTimer = true;
                        string text = $"Doing Combat Stuff for {attack.AttackName} against";
                        for (int i = 0; i < EnemySelected.Count; i++)
                            text += $" { EnemySelected[i].name } & ";

                        text.Remove(text.Length - 2, 2);
                        text += ". . .";
                        Debug.Log(text);
                    }
                }
                break;
            #endregion

            default: Debug.LogError($"{attack.targetingType} not set up."); EndCombat(); return;
        }

        StartCoroutine(StartMiniGame(attack));
    }

    public void StartTurnEnemy(BaseAttackSO attack)
    {
        StartCoroutine(EnemyDelay(attack));
    }

    IEnumerator EnemyDelay(BaseAttackSO attack)
    {
        PlayerCharacter[] attackable = FindObjectsOfType<PlayerCharacter>();
        PlayerSelected.Clear();
        EnemySelected.Clear();
        CurrentPlayer = null;
        StartTimer = false;

        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        switch (attack.targetingType)
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
                System.Random singleRand = new System.Random();
                attackable = attackable.OrderBy(x => singleRand.Next()).ToArray();
                foreach (var obj in attackable)
                {
                    if (!obj.Dead)
                    {
                        PlayerSelected.Add(obj.gameObject);
                        TempPage.SetActive(true);
                        StartTimer = true;

                        character = Instantiate(obj.GetCharacterController);
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against {PlayerSelected[0].name}. . .");
                        break;
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
                foreach (var obj in attackable)
                {
                    if (!obj.Dead)
                    {
                        PlayerSelected.Add(obj.gameObject);
                        character = Instantiate(obj.GetCharacterController);
                    }
                    if (PlayerSelected.Count == attack.numberOFTargets)
                    {
                        TempPage.SetActive(true);
                        StartTimer = true;
                        string text = $"Doing Combat Stuff for {attack.AttackName} against";
                        for (int i = 0; i < PlayerSelected.Count; i++)
                            text += $" { PlayerSelected[i].name } & ";

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
                TempPage.SetActive(true);
                StartTimer = true;
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
                        PlayerSelected.Add(obj.gameObject);
                        TempPage.SetActive(true);
                        StartTimer = true;
                    }
                }

                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, against all alive players. . .");
                break;
            #endregion

            default: Debug.LogError($"{attack.targetingType} not set up."); EndCombat(); yield break;
        }

        StartCoroutine(StartMiniGame(attack));
    }

    IEnumerator StartMiniGame(BaseAttackSO attack)
    {
        float miniGameTime = attack.MiniGameTime;
        Timer.text = miniGameTime.ToString();

        miniGame = Instantiate(attack.CombatPrefab);
        miniGame.transform.SetParent(MiniGameScreen.transform);
        while(!StartTimer) yield return null;

        if (CurrentPlayer)
        {
            foreach (var aug in CurrentPlayer.GetAUGS)
                if(aug.Type == DebuffStackSO.ActivateType.InCombat)
                    aug.DebuffEffect();
        }
        if(PlayerSelected.Count > 0)
        {
            foreach(var player in PlayerSelected)
            {
                foreach(var aug in player.GetComponent<PlayerCharacter>().GetAUGS)
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


        if (CurrentPlayer)
        {
            foreach (var aug in CurrentPlayer.GetAUGS)
                aug.StopEffect();
        }
        if (PlayerSelected.Count > 0)
        {
            foreach (var player in PlayerSelected)
            {
                foreach (var aug in player.GetComponent<PlayerCharacter>().GetAUGS)
                    aug.StopEffect();
            }
        }

        EndCombat();
    }

    public void EndCombat()
    {
        StopAllCoroutines();
        TempPage.SetActive(false);

        miniGame.GetComponentInChildren<CombatMove>().EndMove();

        PlayerSelected.Clear();
        EnemySelected.Clear();
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
}
