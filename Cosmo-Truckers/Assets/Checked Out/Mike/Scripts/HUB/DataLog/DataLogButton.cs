using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataLogButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //TODO add key value to determine if it is locked on awake
    [SerializeField] [TextArea] string dataText;

    DataLogController controller;
    bool locked = false;

    public void OnDeselect(BaseEventData eventData)
    {
        controller.DataYapAura.text = "";
    }

    public void OnSelect(BaseEventData eventData)
    {
        controller.DataYapAura.text = "";
        controller.DataYapAura.text = dataText;
    }

    private void Awake()
    {
        controller = HelperFunctions.FindNearestParentOfType<DataLogController>(transform);
        DetermineLocked();
    }

    private void DetermineLocked()
    {
        //TODO determine locked state
    }
}
