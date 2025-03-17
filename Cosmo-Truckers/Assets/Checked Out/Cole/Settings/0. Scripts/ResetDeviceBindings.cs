using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBindings : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions; 
    [SerializeField] private string targetControlScheme;
    private DuplicateBindingsManager duplicateBindings; 

    private void Start()
    {
        duplicateBindings = GetComponent<DuplicateBindingsManager>();
    }

    /// <summary>
    /// Resets ALL bindings (for example, keyboard, gamepad, etc) in an input action map.  
    /// </summary>
    public void ResetAllBindings()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }

        // duplicateBindings.RemoveAllDuplicates();
    }

    public void ResetControlSchemeBindings()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme)); 
            }
        }

        // duplicateBindings.RemoveAllDuplicates();
    }
}
