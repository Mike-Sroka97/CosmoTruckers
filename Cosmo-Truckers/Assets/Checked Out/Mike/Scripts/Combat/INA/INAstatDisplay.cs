using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class INAstatDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI restorationText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI vigorText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI ReflexText;

    public void UpdateStatDisplay(Character character)
    {
        damageText.text = character.Stats.Damage.ToString();
        restorationText.text = character.Stats.Restoration.ToString();
        defenseText.text = character.Stats.Defense.ToString();
        vigorText.text = character.Stats.Vigor.ToString();
        speedText.text = character.Stats.Speed.ToString();
        ReflexText.text = character.Stats.Reflex.ToString();
    }
}
