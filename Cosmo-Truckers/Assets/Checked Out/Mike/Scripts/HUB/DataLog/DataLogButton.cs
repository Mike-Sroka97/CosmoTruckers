using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataLogButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //TODO add key value to determine if it is locked on awake
    [SerializeField] [TextArea] string dataText;
    [SerializeField] string dataFileKey = "";

    DataLogController controller;

    public void OnDeselect(BaseEventData eventData)
    {
        controller.DataYapAura.text = "";
    }

    public void OnSelect(BaseEventData eventData)
    {
        controller.DataYapAura.text = "";

        if(DetermineUnlocked())
            controller.DataYapAura.text = dataText;
    }

    private void Awake()
    {
        controller = HelperFunctions.FindNearestParentOfType<DataLogController>(transform);

        if (!DetermineUnlocked())
            transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
    }

    private bool DetermineUnlocked()
    {
        return dataFileKey == "" || controller.DataFileUnlocked(dataFileKey);
    }
}
