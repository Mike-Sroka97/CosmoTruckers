using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject TempPage;
    [SerializeField] TMP_Text Text;

    public void StartCombat(BaseAttackSO attack)
    {
        TempPage.SetActive(true);
        Text.text = $"Doing Combat Stuff for {attack.AttackName}. . .";
    }

    public void EndCombat()
    {
        TempPage.SetActive(false);
    }
}
