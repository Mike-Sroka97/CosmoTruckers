using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using TMPro; 

public class DuplicateBindingsManager : MonoBehaviour
{
    private RebindActionUI[] rebindActions;

    private void Awake()
    {
        rebindActions = FindObjectsOfType<RebindActionUI>(); 
    }

    /// <summary>
    /// Call this when a duplicate binding has been detected.
    /// </summary>
    /// <param name="duplicateAction"></param>
    /// <param name="duplicateBindingIndex"></param>
    public void RebindDuplicate(RebindActionUI originalActionUI, InputBinding bindingToFind)
    {
        RebindActionUI duplicateRebindUIObject = null;
        InputAction duplicateAction = null;
        int duplicateIndex = -1; 

        for (int i = 0; i < rebindActions.Length; i++)
        {
            rebindActions[i].ResolveActionAndBinding(out InputAction action, out int bindingIndex);
            InputBinding loopedBinding = action.bindings[bindingIndex];

            if (loopedBinding == bindingToFind && rebindActions[i] != originalActionUI)
            {
                duplicateAction = action; 
                duplicateIndex = bindingIndex;
                duplicateRebindUIObject = rebindActions[i];
                break;
            }
        }

        // Set the duplicate rebind UI object's binding override to null
        if (duplicateRebindUIObject != null)
        {
            InputActionRebindingExtensions.ApplyBindingOverride(duplicateAction, duplicateIndex, "");
            duplicateRebindUIObject.UpdateBindingDisplay();
        }
    }
}
