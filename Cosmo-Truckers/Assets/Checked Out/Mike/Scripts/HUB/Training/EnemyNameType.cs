using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyNameType : MonoBehaviour, ISelectHandler
{
    InaPractice ina;

    private void Awake()
    {
        ina = FindObjectOfType<InaPractice>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ina.TypeEnemyName(GetComponent<TrainingButtonInfo>().CharacterName);
    }
}
