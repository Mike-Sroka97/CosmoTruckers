using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSettingsController : MonoBehaviour
{
    [SerializeField] GameObject selectScreenGO;
    [SerializeField] GameObject videoScreenGO;
    [SerializeField] GameObject audioScreenGO;
    [SerializeField] GameObject gameplayScreenGO;
    [SerializeField] GameObject gameSaveScreenGO;

    [Header("Control Screens and Subscreens")]
    [SerializeField] GameObject controlsMainScreenGO;
    [SerializeField] GameObject keyboardScreenGO;
    [SerializeField] GameObject keyboardMinigameSubScreenGO;
    [SerializeField] GameObject keyboardCombatSubScreenGO;
    [SerializeField] GameObject keyboardUISubScreenGO;
    [SerializeField] GameObject gamepadScreenGO;
    [SerializeField] GameObject[] gamepadLayoutSubScreenGOs;

    private void OnEnable()
    {
        selectScreenGO.SetActive(true);
    }

    /// <summary>
    /// Open the main settings selection screen and close the previous screen
    /// </summary>
    /// <param name="thisScreenGO"></param>
    public void OpenSelectScreen(GameObject thisScreenGO)
    {
        selectScreenGO.SetActive(true);
        thisScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open the video settings screen
    /// </summary>
    public void OpenVideoScreen()
    {
        videoScreenGO.SetActive(true);
        selectScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open the audio settings screen
    /// </summary>
    public void OpenAudioScreen()
    {
        audioScreenGO.SetActive(true);
        selectScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open the gameplay settings screen
    /// </summary>
    public void OpenGameplayScreen()
    {
        gameplayScreenGO.SetActive(true);
        selectScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open the game save settings screen
    /// </summary>
    public void OpenGameSaveScreen()
    {
        gameSaveScreenGO.SetActive(true);
        selectScreenGO.SetActive(false);
    }

    #region Controls Screens
    /// <summary>
    /// Open the main controls screen. By default, this opens the Keyboard Controls screen.
    /// </summary>
    public void OpenControlsScreen()
    {
        controlsMainScreenGO.SetActive(true);
        OpenKeyboardControlScreen();
        selectScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open the keyboard control settings screen
    /// </summary>
    public void OpenKeyboardControlScreen()
    {
        keyboardScreenGO.SetActive(true);
        keyboardMinigameSubScreenGO.SetActive(true);
        keyboardCombatSubScreenGO.SetActive(false);
        keyboardUISubScreenGO.SetActive(false);

        // Disable gamepad screens
        gamepadScreenGO.SetActive(false); 
    }

    /// <summary>
    /// Open the gamepad control settings screen
    /// </summary>
    public void OpenGamepadControlScreen()
    {
        gamepadScreenGO.SetActive(true);
        // Load which layout the player chose last and enable it here

        // Disable keyboard screens
        keyboardScreenGO.SetActive(false);
        keyboardMinigameSubScreenGO.SetActive(false);
        keyboardCombatSubScreenGO.SetActive(false);
        keyboardUISubScreenGO.SetActive(false);
    }

    /// <summary>
    /// Open a subscreen for the keyboard control settings
    /// </summary>
    public void OpenMinigameSubScreen(ControlSubScreens subScreenToOpen, ControlSubScreens thisSubScreen)
    {
        GetKeyboardSubScreen(subScreenToOpen).SetActive(true);
        GetKeyboardSubScreen(thisSubScreen).SetActive(false);
    }

    private GameObject GetKeyboardSubScreen(ControlSubScreens subScreen)
    {
        switch (subScreen)
        {
            case ControlSubScreens.Combat:
                return keyboardCombatSubScreenGO; 
            case ControlSubScreens.UI:
                return keyboardUISubScreenGO;
            case ControlSubScreens.Minigame:
            default:
                return keyboardMinigameSubScreenGO; 
        }
    }

    public enum ControlSubScreens
    {
        Minigame,
        Combat,
        UI,
    }
    #endregion
}
