using UnityEngine;
using UnityEngine.InputSystem;
using Steamworks; 

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

    #region Combat Input Values
    public bool AttackDescPressed { get; private set; }
    public bool AugNavRightPressed { get; private set; }
    public bool AugNavLeftPressed { get; private set; }
    public bool EmoteMenuPressed { get; private set; }
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
    public InputAction AttackDescOpenAction { get; private set; }
    public InputAction AugNavRightAction { get; private set; }
    public InputAction AugNavLeftAction { get; private set; }
    public InputAction EmoteMenuAction { get; private set; }
    #endregion

    [HideInInspector]
    public PlayerInput PlayerInput { get; private set; }
    public float GamePadMoveValueFloor { get; private set; } = 0.25f; // The gamepad joystick must be at a position above this value for the input to be considered for that axis
    
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
        // Disable the Steam API
        if (SteamManager.Initialized)
        {
            SteamInput.Shutdown(); 
        }

        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }

        keyChecker = GetComponent<KeyChecker>();
        SetupInputActions();
    }

    private void Update()
    {
        UpdateActions();

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
        AttackDescOpenAction = PlayerInput.actions[PlayerActions.AttackDescriptionOpen.ToString()];
        AugNavRightAction = PlayerInput.actions[PlayerActions.AugmentNavigateRight.ToString()];
        AugNavLeftAction = PlayerInput.actions[PlayerActions.AugmentNavigateLeft.ToString()];
        EmoteMenuAction = PlayerInput.actions[PlayerActions.EmoteMenuOpen.ToString()];
    }

    #region Control Scheme  
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
        // Check each key in the key checker if it was pressed this frame
        foreach (Key key in keyChecker.allKeys)
        {
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                return true;
            }
        }

        // Check the mouse buttons to determine if they were pressed this frame
        if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.middleButton.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
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
    #endregion

    #region Actions
    private void UpdateActions()
    {
        // MOVEMENT
        MoveInputs(); 
        JumpPressed = JumpAction.WasPressedThisFrame();
        JumpHeld = JumpAction.IsPressed(); 
        AttackPressed = AttackAction.WasPressedThisFrame();
        AttackHeld = AttackAction.IsPressed();
        SpecialPressed = SpecialAction.WasPressedThisFrame();
        SpecialHeld = SpecialAction.IsPressed();

        // COMBAT
        AttackDescPressed= AttackAction.WasPressedThisFrame();
        AugNavRightPressed = AugNavRightAction.WasPressedThisFrame();
        AugNavLeftPressed = AugNavLeftAction.WasPressedThisFrame();
        EmoteMenuPressed = EmoteMenuAction.WasPressedThisFrame();

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
        Vector2 originalReadValue = moveValues; 
        float xMultiplier = moveValues.x < 0 ? -1 : 1;
        float yMultiplier = moveValues.y < 0 ? -1 : 1;

        // If move values is actually receiving data, continue
        if (moveValues.magnitude > 0)
        {
            if (moveValues.x != 0)
            {
                if (Mathf.Abs(moveValues.x) < GamePadMoveValueFloor)
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
                if (Mathf.Abs(moveValues.y) < GamePadMoveValueFloor)
                {
                    moveValues.y = 0;
                }
                else
                {
                    moveValues.y = 1 * yMultiplier;
                }
            }

            // We are at the end of modifying the move values. At least one value was originally > 0, but we have the Vector2 as 0,0.
            // Set one value to be higher than 0. 
            if (originalReadValue.magnitude > 0 && moveValues.magnitude == 0)
            {
                // X has value, Y does not
                if (originalReadValue.x != 0 && originalReadValue.y == 0)
                {
                    moveValues.x = 1 * xMultiplier;
                }
                // Y has value, X does not
                else if (originalReadValue.x == 0 && originalReadValue.y != 0)
                {
                    moveValues.y = 1 * yMultiplier;
                }
                // Both vectors have a value
                else
                {
                    // Choose a higher value and set that one
                    if (Mathf.Abs(originalReadValue.x) > Mathf.Abs(originalReadValue.y))
                    {
                        moveValues.x = 1 * xMultiplier;
                    }
                    else
                    {
                        moveValues.y = 1 * yMultiplier;
                    }
                }
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
        AttackDescriptionOpen, 
        AugmentNavigateRight, 
        AugmentNavigateLeft,
        EmoteMenuOpen, 
    }

    private void OnDestroy()
    {
        // Populate this when needed
    }
}
