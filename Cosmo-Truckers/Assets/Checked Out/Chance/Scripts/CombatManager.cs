using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject TempPage;
    [SerializeField] GameObject MiniGameScreen;
    GameObject miniGame;
    [SerializeField] TMP_Text Timer;
 
    [SerializeField] List<GameObject> EnemySelected;

    public void StartCombat(BaseAttackSO attack)
    {
        EnemySelected.Clear();

        switch (attack.targetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                TempPage.SetActive(true);
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, no target. . .");
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                TempPage.SetActive(true);
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, self target. . .");
                break;
            #endregion
            #region Single Target
            case EnumManager.TargetingType.Single_Target:
                foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
                {
                    Button button = obj.gameObject.GetComponentInChildren<Button>();
                    button.interactable = true;
                    button.onClick.AddListener(delegate
                    {
                        print(obj.gameObject.name);
                        EnemySelected.Add(obj.gameObject);
                        TempPage.SetActive(true);
                        Debug.Log($"Doing Combat Stuff for {attack.AttackName} against {EnemySelected[0].name}. . .");
                    });
                }
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                TempPage.SetActive(true);
                Debug.Log($"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .");
                break;
            #endregion
            #region Multi Target Choice
            case EnumManager.TargetingType.Multi_Target_Choice:
                foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
                {
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

            default: Debug.LogError($"{attack.targetingType} not set up."); break;
        }

        StartCoroutine(StartMiniGame(attack));
    }

    IEnumerator StartMiniGame(BaseAttackSO attack)
    {
        float miniGameTime = 60.0f; //TODO add in time for each mini Game
        Timer.text = miniGameTime.ToString();

        miniGame = Instantiate(attack.CombatPrefab, MiniGameScreen.transform, false);
        miniGame.transform.localScale = MiniGameScreen.transform.localScale;

        while(miniGameTime >= 0)
        {
            miniGameTime -= Time.deltaTime;
            Timer.text = ((int)miniGameTime).ToString();
            yield return null;
        }

        EndCombat();
    }

    public void EndCombat()
    {
        StopAllCoroutines();
        TempPage.SetActive(false);
        Destroy(miniGame);

        foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
        {
            Button button = obj.gameObject.GetComponentInChildren<Button>();
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }

        FindObjectOfType<TurnOrder>().EndTurn();
    }
}
