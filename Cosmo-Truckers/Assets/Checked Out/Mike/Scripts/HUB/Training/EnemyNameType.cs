using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyNameType : MonoBehaviour, ISelectHandler
{
    [SerializeField] TextMeshProUGUI tmpToUpdate;
    [SerializeField] bool updateEnemyId;
    InaPractice ina;

    private void Awake()
    {
        ina = FindObjectOfType<InaPractice>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ina.TypeEnemyName(GetComponent<TrainingButtonInfo>().CharacterName, tmpToUpdate);

        if (updateEnemyId)
            ina.SetEnemyId(GetComponent<TrainingButtonInfo>().EnemyID);
    }
}
