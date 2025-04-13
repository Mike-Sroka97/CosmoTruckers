using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    #region  Minigame Input Values
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool AttackHeld { get; private set; }
    public bool SpecialPressed { get; private set; }
    public bool SpecialHeld { get; private set; }
    #endregion

    #region  UI Input Values
    public bool SelectPressed { get; private set; }
    public bool BackPressed { get; private set; }
    #endregion

    #region Input Actions
    public InputAction MoveAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction AttackAction { get; private set; }
    public InputAction SpecialAction { get; private set; }
    public InputAction SelectAction { get; private set; }
    public InputAction BackAction { get; private set; }
    #endregion

    [HideInInspector]
    public PlayerInput PlayerInput { get; private set; }
    private const string RebindsKey = "rebinds";

    //Set instance or remove object
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            PlayerInput = GetComponent<PlayerInput>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

        if (string.IsNullOrEmpty(rebinds))
        {
            return; 
        }
        else
        {
            PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }

        SetupInputActions();
        SetupMinigameInputs();
        SetupUiInputs();
    }

    private void Update()
    {
        UpdateMinigameAcitons();
    }

    #region Setup
    public void SetupInputActions()
    {
        MoveAction = PlayerInput.actions[PlayerActions.Move.ToString()];
        JumpAction = PlayerInput.actions[PlayerActions.Jump.ToString()];
        AttackAction = PlayerInput.actions[PlayerActions.Attack.ToString()];
        SpecialAction = PlayerInput.actions[PlayerActions.Special.ToString()]; 
        SelectAction = PlayerInput.actions[PlayerActions.Select.ToString()]; 
        BackAction = PlayerInput.actions[PlayerActions.Back.ToString()];  
    }

    public void SetupMinigameInputs()
    {
        // MINIGAME - MOVE

        
        // MINIGAME - JUMP
        JumpAction.performed += JumpPerformed; 
        JumpAction.canceled += JumpCancelled;

        // MINIGAME - ATTACK
        AttackAction.performed += AttackPerformed;
        AttackAction.canceled += AttackCancelled;

        // MINIGAME - Special
        SpecialAction.performed += SpecialPerformed;
        SpecialAction.canceled += SpecialCancelled;
    }

    public void SetupUiInputs()
    {
        // UI
        SelectPressed = SelectAction.WasPressedThisFrame();
        BackPressed = BackAction.WasPressedThisFrame();
    }
    #endregion

    #region Minigame
    private void UpdateMinigameAcitons()
    {
        MoveInput = MoveAction.ReadValue<Vector2>();
        JumpPressed = JumpAction.WasPressedThisFrame();
        AttackPressed = AttackAction.WasPressedThisFrame();
        SpecialPressed = SpecialAction.WasPressedThisFrame();
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        JumpHeld = true;
    }

    private void JumpCancelled(InputAction.CallbackContext context)
    {
        JumpPressed = false;
        JumpHeld = false;
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        AttackHeld = true;
    }

    private void AttackCancelled(InputAction.CallbackContext context)
    {
        AttackPressed = false;
        AttackHeld = false;
    }

    private void SpecialPerformed(InputAction.CallbackContext context)
    {
        SpecialHeld = true;
    }

    private void SpecialCancelled(InputAction.CallbackContext context)
    {
        SpecialPressed = false;
        SpecialHeld = false;
    }
    #endregion

    public enum PlayerActions
    {
        Move, 
        Jump, 
        Attack,
        Special,
        Select,
        Back, 
        ActionSwap,
    }
}
