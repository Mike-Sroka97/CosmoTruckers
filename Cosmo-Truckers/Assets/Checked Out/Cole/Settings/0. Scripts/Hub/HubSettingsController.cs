using System;
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
    [SerializeField] GameObject[] keyboardSubScreenGOs;
    [SerializeField] GameObject gamepadScreenGO;
    [SerializeField] GameObject[] gamepadLayoutSubScreenGOs;

    [HideInInspector] public SettingsData SettingsData;
    private int currentSubScreen = 0;

    private void OnEnable()
    {
        SettingsData = SettingsManager.LoadSettingsData();
        selectScreenGO.SetActive(true);

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
    public void OpenControlsScreen(bool open)
    {
        if (open)
        {
            controlsMainScreenGO.SetActive(true);
            OpenKeyboardControlScreen(true);
            selectScreenGO.SetActive(false);
        }
        else
        {
            selectScreenGO.SetActive(true);
            OpenKeyboardControlScreen(false);
            OpenGamepadControlScreen(false); 
            controlsMainScreenGO.SetActive(false);
        }
    }

    /// <summary>
    /// Open the keyboard control settings screen. By default, this opens the Minigame Sub Screen
    /// </summary>
    public void OpenKeyboardControlScreen(bool open)
    {
        if (open)
        {
            // Enable keyboard screen and minigame sub screen, disable all other sub screens
            keyboardScreenGO.SetActive(true);
            for (int i = 0; i < keyboardSubScreenGOs.Length; i++)
            {
                if (i == 0)
                {
                    keyboardSubScreenGOs[i].SetActive(true);
                }
                else
                {
                    keyboardSubScreenGOs[i].SetActive(false);
                }
            }
        }
        else
        {
            keyboardScreenGO.SetActive(false);
            CloseKeyboardSubScreens(); 
        }
    }

    public void NextKeyboardSubScreen(bool right)
    {
        if (right)
        {
            currentSubScreen++; 

            if (currentSubScreen == keyboardSubScreenGOs.Length)
            {
                keyboardSubScreenGOs[currentSubScreen - 1].SetActive(false);
                keyboardSubScreenGOs[0].SetActive(true);
                currentSubScreen = 0;
            }
            else
            {
                keyboardSubScreenGOs[currentSubScreen].SetActive(true);
                keyboardSubScreenGOs[currentSubScreen - 1].SetActive(false);
            }
        }
        else
        {
            currentSubScreen--; 
            if (currentSubScreen == -1)
            {
                currentSubScreen = keyboardSubScreenGOs.Length - 1;
                keyboardSubScreenGOs[currentSubScreen].SetActive(true);
                keyboardSubScreenGOs[0].SetActive(false);
            }
            else
            {
                keyboardSubScreenGOs[currentSubScreen].SetActive(true);
                keyboardSubScreenGOs[currentSubScreen + 1].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Open a subscreen for the keyboard control settings
    /// </summary>
    public void OpenMinigameSubScreen(GameObject subScreenToOpen, GameObject thisSubScreen)
    {
        subScreenToOpen.SetActive(true);
        thisSubScreen.SetActive(false);
    }

    private void CloseKeyboardSubScreens()
    {
        foreach (GameObject screen in keyboardSubScreenGOs)
            screen.SetActive(false);
    }

    /// <summary>
    /// Swap between Keyboard or Gamepad control screens
    /// </summary>
    public void SwapControlScreens()
    {
        if (!gamepadScreenGO.activeSelf)
        {
            OpenGamepadControlScreen(true); 
            OpenKeyboardControlScreen(false);
        }
        else
        {
            OpenKeyboardControlScreen(true); 
            OpenGamepadControlScreen(false);
        }
    }

    /// <summary>
    /// Open the Gamepad control settings screen.
    /// </summary>
    public void OpenGamepadControlScreen(bool open)
    {
        if (open)
        {
            // Enable keyboard screen and minigame sub screen, disable all other sub screens
            gamepadScreenGO.SetActive(true);
            for (int i = 0; i < gamepadLayoutSubScreenGOs.Length; i++)
            {
                if (i == SettingsData.GamepadLayout)
                {
                    gamepadLayoutSubScreenGOs[i].SetActive(true);
                }
                else
                {
                    gamepadLayoutSubScreenGOs[i].SetActive(false);
                }
            }
        }
        else
        {
            gamepadScreenGO.SetActive(false);
            CloseGamepadSubScreens();
        }
    }

    public void NextGamepadLayout(bool right)
    {
        int gamePadLayout = SettingsData.GamepadLayout; 

        if (right)
        {
            gamePadLayout++;

            if (gamePadLayout == gamepadLayoutSubScreenGOs.Length)
            {
                gamepadLayoutSubScreenGOs[gamePadLayout - 1].SetActive(false);
                gamepadLayoutSubScreenGOs[0].SetActive(true);
                gamePadLayout = 0;
            }
            else
            {
                gamepadLayoutSubScreenGOs[gamePadLayout].SetActive(true);
                gamepadLayoutSubScreenGOs[gamePadLayout - 1].SetActive(false);
            }
        }
        else
        {
            gamePadLayout--;
            if (gamePadLayout == -1)
            {
                gamePadLayout = keyboardSubScreenGOs.Length - 1;
                gamepadLayoutSubScreenGOs[gamePadLayout].SetActive(true);
                gamepadLayoutSubScreenGOs[0].SetActive(false);
            }
            else
            {
                gamepadLayoutSubScreenGOs[gamePadLayout].SetActive(true);
                gamepadLayoutSubScreenGOs[gamePadLayout + 1].SetActive(false);
            }
        }

        // Save the settings data
        SettingsData = SettingsData.SaveGamepadLayoutSelection(gamePadLayout); 
    }

    private void CloseGamepadSubScreens()
    {
        foreach (GameObject screen in gamepadLayoutSubScreenGOs)
            screen.SetActive(false);
    }
    #endregion
}
