using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControlLayout : MonoBehaviour
{
    public GamepadBinding[] ChangingLayoutBindings; 

    /// <summary>
    /// Set the action to THIS binding<br></br>
    /// This method is called every time the user swaps between Gamepad layouts on each binding that changes
    /// </summary>
    public void SetBinding()
    {
        foreach (GamepadBinding layoutItem in ChangingLayoutBindings)
        {
            InputActionRebindingExtensions.ApplyBindingOverride(layoutItem.Action, layoutItem.Binding);
        }
    }
}

[Serializable]
public class GamepadBinding
{
    public InputActionReference Action;
    public InputBinding Binding;
}
