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
    public bool JumpReleased { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool SpecialPressed { get; private set; }
    public bool SpecialHeld { get; private set; }
    public bool SpecialReleased { get; private set; }
    #endregion

    #region  UI Input Values
    public bool SelectPressed { get; private set; }
    public bool BackPressed { get; private set; }
    public bool ActionSwapPressed { get; private set; }
    #endregion

    #region Input Actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction specialAction;
    private InputAction selectAction;
    private InputAction backAction;
    private InputAction actionSwapAction;
    #endregion

    public bool RebindingKey { get; private set; } = false; 
    private PlayerInput playerInput;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private TMP_Text textToModify;
    private InputActionReference currentInputAction;
    private const string RebindsKey = "rebinds";

    //Set instance or remove object
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        SetupInputActions(); 

        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty); 

        if (string.IsNullOrEmpty(rebinds))
        {
            return; 
        }
        else
        {
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    private void Update()
    {
        UpdateInputs(); 
    }

    public void SetupInputActions()
    {
        moveAction = playerInput.actions[PlayerActions.Move.ToString()];
        jumpAction = playerInput.actions[PlayerActions.Jump.ToString()];
        attackAction = playerInput.actions[PlayerActions.Attack.ToString()];
        specialAction = playerInput.actions[PlayerActions.Special.ToString()]; 
        selectAction = playerInput.actions[PlayerActions.Select.ToString()]; 
        backAction = playerInput.actions[PlayerActions.Back.ToString()];  
        actionSwapAction = playerInput.actions[PlayerActions.ActionSwap.ToString()];  
    }

    public void UpdateInputs()
    {
        // MINIGAME
        MoveInput = moveAction.ReadValue<Vector2>(); 
        JumpPressed = jumpAction.WasPressedThisFrame();
        JumpHeld = jumpAction.IsPressed(); 
        JumpReleased = jumpAction.WasReleasedThisFrame();
        AttackPressed = attackAction.WasPressedThisFrame(); 
        SpecialPressed = specialAction.WasPressedThisFrame();
        SpecialHeld = specialAction.IsPressed();
        SpecialReleased = specialAction.WasReleasedThisFrame();

        // UI
        SelectPressed = selectAction.WasPressedThisFrame();
        BackPressed = backAction.WasPressedThisFrame();
        ActionSwapPressed = actionSwapAction.WasPressedThisFrame();
    }

    private enum PlayerActionMaps
    {
        Player, 
        UI,
    }

    private enum PlayerActions
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
