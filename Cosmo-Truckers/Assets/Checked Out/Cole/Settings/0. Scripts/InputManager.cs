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
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction specialAction;
    private InputAction selectAction;
    private InputAction backAction;
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
        moveAction = PlayerInput.actions[PlayerActions.Move.ToString()];
        jumpAction = PlayerInput.actions[PlayerActions.Jump.ToString()];
        attackAction = PlayerInput.actions[PlayerActions.Attack.ToString()];
        specialAction = PlayerInput.actions[PlayerActions.Special.ToString()]; 
        selectAction = PlayerInput.actions[PlayerActions.Select.ToString()]; 
        backAction = PlayerInput.actions[PlayerActions.Back.ToString()];  
    }

    public void SetupMinigameInputs()
    {
        // MINIGAME - MOVE

        
        // MINIGAME - JUMP
        jumpAction.performed += JumpPerformed; 
        jumpAction.canceled += JumpCancelled;

        // MINIGAME - ATTACK
        attackAction.performed += AttackPerformed;
        attackAction.canceled += AttackCancelled;

        // MINIGAME - Special
        specialAction.performed += SpecialPerformed;
        specialAction.canceled += SpecialCancelled;
    }

    public void SetupUiInputs()
    {
        // UI
        SelectPressed = selectAction.WasPressedThisFrame();
        BackPressed = backAction.WasPressedThisFrame();
    }
    #endregion

    #region Minigame
    private void UpdateMinigameAcitons()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        JumpPressed = jumpAction.WasPressedThisFrame();
        AttackPressed = attackAction.WasPressedThisFrame();
        SpecialPressed = specialAction.WasPressedThisFrame();
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
