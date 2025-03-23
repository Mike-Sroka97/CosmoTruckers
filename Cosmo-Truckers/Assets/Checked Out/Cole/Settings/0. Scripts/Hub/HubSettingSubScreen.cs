using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubSettingSubScreen : MonoBehaviour
{
    [Header("Constant Buttons")]
    [SerializeField] Button screenLeftButton;
    [SerializeField] Button screenRightButton;
    [SerializeField] Button subScreenLeftButton;
    [SerializeField] Button subScreenRightButton;

    [Header("New Down/Up Buttons")]
    [SerializeField] Button screenLeftButtonUp;
    [SerializeField] Button screenRightButtonUp;
    [SerializeField] Button subLeftButtonDown;
    [SerializeField] Button subRightButtonDown;

    private void OnEnable()
    {
        // Setup main screen left/right new navigations
        screenLeftButton.navigation = NewNavigation(screenLeftButton.navigation, newUp: screenLeftButtonUp);
        screenRightButton.navigation = NewNavigation(screenRightButton.navigation, newUp: screenRightButtonUp);

        // Setup sub screen left/right new navigations
        subScreenLeftButton.navigation = NewNavigation(subScreenLeftButton.navigation, newDown: subLeftButtonDown);
        subScreenRightButton.navigation = NewNavigation(subScreenRightButton.navigation, newDown: subRightButtonDown);
    }

    /// <summary>
    /// Return a new navigation for a button
    /// </summary>
    /// <param name="originalNavigation"></param>
    /// <param name="newMode"></param>
    /// <param name="newLeft"></param>
    /// <param name="newRight"></param>
    /// <param name="newUp"></param>
    /// <param name="newDown"></param>
    /// <returns></returns>
    private Navigation NewNavigation(Navigation originalNavigation, Navigation.Mode newMode = Navigation.Mode.Explicit, 
        Button newLeft = null, Button newRight = null, Button newUp = null, Button newDown = null)
    {
        Navigation newNavigation = new Navigation();
        newNavigation.mode = newMode;

        newNavigation.selectOnLeft = newLeft != null ? newLeft : originalNavigation.selectOnLeft; 
        newNavigation.selectOnRight = newRight != null ? newRight : originalNavigation.selectOnRight; 
        newNavigation.selectOnUp = newUp != null ? newUp : originalNavigation.selectOnUp; 
        newNavigation.selectOnDown = newDown != null ? newDown : originalNavigation.selectOnDown;

        return newNavigation; 
    }
}
