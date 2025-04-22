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
   
    /// <summary>
    /// The gamepad joystick must be at a position above this value for the input to be considered for that axis
    /// </summary>
    public float GamePadMoveValueFloor = 0.25f;
    [SerializeField] private string currentScheme = "Keyboard";
    
    private float lastSchemeSwitchTime = 0f;
    private float switchSchemeCooldown = 0.5f;
    private const string RebindsKey = "rebinds";
    private KeyChecker keyChecker; 

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

        keyChecker = GetComponent<KeyChecker>();
        SetupInputActions();
    }

    private void Update()
    {
        UpdateMinigameAcitons();

        // Check and update control scheme
        SwapControlScheme();
    }

    public void SetupInputActions()
    {
        MoveAction = PlayerInput.actions[PlayerActions.Move.ToString()];
        JumpAction = PlayerInput.actions[PlayerActions.Jump.ToString()];
        AttackAction = PlayerInput.actions[PlayerActions.Attack.ToString()];
        SpecialAction = PlayerInput.actions[PlayerActions.Special.ToString()];
        SelectAction = PlayerInput.actions[PlayerActions.Select.ToString()];
        BackAction = PlayerInput.actions[PlayerActions.Back.ToString()];
    }

    # region Control Scheme  

    private void TrySwitchScheme(string scheme)
    {
        if (Time.time - lastSchemeSwitchTime < switchSchemeCooldown)
            return;

        SwitchControlScheme(scheme);
        lastSchemeSwitchTime = Time.time;
    }
    private void SwitchControlScheme(string newScheme)
    {
        if (PlayerInput == null)
            return;

        InputDevice deviceToUse = null;

        if (newScheme == "Keyboard")
        {
            deviceToUse = Keyboard.current;
        }
        else if (newScheme == "Gamepad")
        {
            deviceToUse = Gamepad.current;
        }

        if (deviceToUse != null)
        {
            PlayerInput.SwitchCurrentControlScheme(newScheme, deviceToUse);
            currentScheme = newScheme;
        }
    }

    private void SwapControlScheme()
    {
        bool gamepadInput = TrackGamePadActions(); 
        bool keyboardInput = TrackKeyboardActions();

        if (gamepadInput)
        {
            if (currentScheme != "Gamepad")
                TrySwitchScheme("Gamepad");
        }
        else if (keyboardInput)
        {
            if (currentScheme != "Keyboard")
                TrySwitchScheme("Keyboard");
        }
    }

    private bool TrackGamePadActions()
    {
        Gamepad gamePad = Gamepad.current;
        if (gamePad == null) return false;

        // Check if any button has been pressed
        return gamePad.buttonSouth.wasPressedThisFrame
            || gamePad.buttonNorth.wasPressedThisFrame
            || gamePad.buttonEast.wasPressedThisFrame
            || gamePad.buttonWest.wasPressedThisFrame
            || gamePad.leftStick.ReadValue().magnitude > 0.2f
            || gamePad.rightStick.ReadValue().magnitude > 0.2f
            || gamePad.dpad.ReadValue().magnitude > 0.2f
            || gamePad.rightTrigger.ReadValue() > 0.1f
            || gamePad.leftTrigger.ReadValue() > 0.1f
            || gamePad.rightShoulder.wasPressedThisFrame
            || gamePad.leftShoulder.wasPressedThisFrame
            || gamePad.startButton.wasPressedThisFrame
            || gamePad.selectButton.wasPressedThisFrame;
    }
    private bool TrackKeyboardActions()
    {
        foreach (Key key in keyChecker.allKeys)
        {
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                return true; 
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.middleButton.wasPressedThisFrame)
        {
            return true; 
        }

        return false; 
    }

    #endregion

    #region Minigame
    private void UpdateMinigameAcitons()
    {
        // MOVEMENT
        MoveInputs(); 
        JumpPressed = JumpAction.WasPressedThisFrame();
        JumpHeld = JumpAction.IsPressed(); 
        AttackPressed = AttackAction.WasPressedThisFrame();
        AttackHeld = AttackAction.IsPressed();
        SpecialPressed = SpecialAction.WasPressedThisFrame();
        SpecialHeld = SpecialAction.IsPressed();

        // UI
        SelectPressed = SelectAction.WasPressedThisFrame();
        BackPressed = BackAction.WasPressedThisFrame();
    }
    /// <summary>
    /// Define the Move Inputs with a floor for what is considered an input
    /// This fixes issues with aiming in one direction when the other is slightly pressed on a Joystick
    /// </summary>
    private void MoveInputs()
    {
        Vector2 moveValues = MoveAction.ReadValue<Vector2>();
        float xMultiplier = moveValues.x < 0 ? -1 : 1;
        float yMultiplier = moveValues.y < 0 ? -1 : 1;

        if (moveValues.x != 0)
        {
            if ((moveValues.x < 0 && moveValues.x > GamePadMoveValueFloor) || (moveValues.x > 0 && moveValues.x < GamePadMoveValueFloor))
            {
                moveValues.x = 0;
            }
            else
            {
                moveValues.x = 1 * xMultiplier; 
            }
        }
        if (moveValues.y != 0)
        {
            if ((moveValues.y < 0 && moveValues.y > GamePadMoveValueFloor) || (moveValues.y > 0 && moveValues.y < GamePadMoveValueFloor))
            {
                moveValues.y = 0;
            }
            else
            {
                moveValues.y = 1 * yMultiplier;
            }
        }

        MoveInput = moveValues; 
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

    private void OnDestroy()
    {
        // Populate this when needed
    }
}
