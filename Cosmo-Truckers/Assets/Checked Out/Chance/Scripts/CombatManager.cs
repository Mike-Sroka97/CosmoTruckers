using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject TempPage;
    [SerializeField] TMP_Text Text;

    [SerializeField] List<GameObject> EnemySelected;

    public void StartCombat(BaseAttackSO attack)
    {
        EnemySelected.Clear();

        switch (attack.targetingType)
        {
            #region No Target
            case EnumManager.TargetingType.No_Target:
                TempPage.SetActive(true);
                Text.text = $"Doing Combat Stuff for {attack.AttackName}, no target. . .";
                break;
            #endregion
            #region Self Target
            case EnumManager.TargetingType.Self_Target:
                TempPage.SetActive(true);
                Text.text = $"Doing Combat Stuff for {attack.AttackName}, self target. . .";
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
                        Text.text = $"Doing Combat Stuff for {attack.AttackName} against {EnemySelected[0].name}. . .";
                    });
                }
                break;
            #endregion
            #region Multi Target Cone
            case EnumManager.TargetingType.Multi_Target_Cone:
                TempPage.SetActive(true);
                Text.text = $"Doing Combat Stuff for {attack.AttackName}, Cone attack. . .";
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
                            Text.text = $"Doing Combat Stuff for {attack.AttackName} against";
                            for(int i = 0; i < EnemySelected.Count; i++)
                                Text.text += $" { EnemySelected[i].name } & ";

                            Text.text.Remove(Text.text.Length - 2, 2);
                            Text.text += ". . .";
                        }
                    });
                }
                break;
            #endregion
            #region AOE
            case EnumManager.TargetingType.AOE:
                TempPage.SetActive(true);
                Text.text = $"Doing Combat Stuff for {attack.AttackName}, AOE. . .";
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
                        Text.text = $"Doing Combat Stuff for {attack.AttackName} against";
                        for (int i = 0; i < EnemySelected.Count; i++)
                            Text.text += $" { EnemySelected[i].name } & ";

                        Text.text.Remove(Text.text.Length - 2, 2);
                        Text.text += ". . .";
                    }
                }
                break;
            #endregion

            default: Debug.LogError($"{attack.targetingType} not set up."); break;
        }
    }

    public void EndCombat()
    {
        TempPage.SetActive(false);

        foreach (var obj in FindObjectOfType<EnemyManager>().Enemies)
        {
            Button button = obj.gameObject.GetComponentInChildren<Button>();
            button.interactable = false;
            button.onClick.RemoveAllListeners();
        }
    }
}
