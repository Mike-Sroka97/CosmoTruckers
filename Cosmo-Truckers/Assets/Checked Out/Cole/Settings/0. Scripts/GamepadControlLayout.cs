using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControlLayout : MonoBehaviour
{
    private RebindSaveLoad saveLoad; 
    public GamepadBinding[] ChangingLayoutBindings;

    private void Awake()
    {
        saveLoad = GetComponent<RebindSaveLoad>(); 
    }

    private void OnEnable()
    {
        SetBinding();
    }

    /// <summary>
    /// Set the action to THIS binding<br></br>
    /// This method is called every time the user swaps between Gamepad layouts on each binding that changes
    /// </summary>
    public void SetBinding()
    {
        foreach (GamepadBinding changingBinding in ChangingLayoutBindings)
        {
            string newBindingPath = $"<Gamepad>/{changingBinding.NewBindingPath}"; 
            InputAction action = changingBinding.Action.action;
            InputActionRebindingExtensions.ApplyBindingOverride(action, changingBinding.BindingIndex, newBindingPath);
        }

        saveLoad.SavePlayerSettings(); 
    }
}

[Serializable]
public class GamepadBinding
{
    public InputActionReference Action;
    public int BindingIndex = 0;
    public string NewBindingPath = string.Empty;
}

/*  POSSIBLE BINDINGS FOR GAMEPAD
    buttonEast
    buttonNorth,
    buttonSouth,
    buttonWest,
    dpad/down,
    dpad/left
    dpad/right
    dpad/up
    leftShoulder
    leftStick/down
    leftStick/left
    leftStick/right
    leftStick/up
    leftStickPress
    leftTrigger
    rightShoulder
    rightStick/down
    rightStick/left
    rightStick/right
    rightStick/up
    rightStickPress
    rightTrigger
    select
    start
    touchpadButton
    leftTriggerButton
    rightTriggerButton
    systemButton
    leftStick
    rightStick
    dpad
*/ 