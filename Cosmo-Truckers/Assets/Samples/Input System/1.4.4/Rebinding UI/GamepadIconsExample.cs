using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class GamepadIconsExample : MonoBehaviour
    {
        public GamepadIcons xbox;
        public GamepadIcons ps4;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            bool containsPlayStationDevice = false;
            bool containsSwitchDevice = false; 

            // Determine what type of controller is plugged in
            foreach (InputDevice device in InputSystem.devices)
            {
                if (device.description.deviceClass.Contains("Dual"))
                {
                    containsPlayStationDevice = true;
                    break; 
                }
                else if (device.description.deviceClass.Contains("Switch"))
                {
                    containsSwitchDevice = true;
                    break;
                }
            }
            string[] joyStickNames = Input.GetJoystickNames();
            for (int i = 0; i < joyStickNames.Length; i++)
            {
                if (joyStickNames[i].Contains("Dual"))
                {
                    containsPlayStationDevice = true;
                    break; 
                }
                else if (joyStickNames[i].Contains("Pro"))
                {
                    containsSwitchDevice = true;
                    break;
                }
            }

            // DualSense Wireless Controller
            // Pro Controller

            var icon = default(Sprite);
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad") || containsPlayStationDevice)
                icon = ps4.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad") && !containsPlayStationDevice && !containsSwitchDevice)
                icon = xbox.GetSprite(controlPath);

            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public struct GamepadIcons
        {
            public Sprite buttonSouth;
            public Sprite buttonNorth;
            public Sprite buttonEast;
            public Sprite buttonWest;
            public Sprite startButton;
            public Sprite selectButton;
            public Sprite leftTrigger;
            public Sprite rightTrigger;
            public Sprite leftShoulder;
            public Sprite rightShoulder;
            public Sprite dpad;
            public Sprite dpadUp;
            public Sprite dpadDown;
            public Sprite dpadLeft;
            public Sprite dpadRight;
            public Sprite leftStickUp;
            public Sprite leftStickDown;
            public Sprite leftStickLeft;
            public Sprite leftStickRight;
            public Sprite leftStickPress;
            public Sprite rightStickUp;
            public Sprite rightStickDown;
            public Sprite rightStickLeft;
            public Sprite rightStickRight;
            public Sprite rightStickPress;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.

                switch (controlPath)
                {
                    case "buttonSouth": return buttonSouth;
                    case "buttonNorth": return buttonNorth;
                    case "buttonEast": return buttonEast;
                    case "buttonWest": return buttonWest;
                    case "start": return startButton;
                    case "select": return selectButton;
                    case "leftTrigger": return leftTrigger;
                    case "rightTrigger": return rightTrigger;
                    case "leftShoulder": return leftShoulder;
                    case "rightShoulder": return rightShoulder;

                    // DPAD                
                    case "dpad": return dpad;
                    case "dpad/up": return dpadUp;
                    case "dpad/down": return dpadDown;
                    case "dpad/left": return dpadLeft;
                    case "dpad/right": return dpadRight;
                    
                    // Left Stick
                    case "leftStick/left": return leftStickLeft;
                    case "leftStick/right": return leftStickRight;
                    case "leftStick/up": return leftStickUp;
                    case "leftStick/down": return leftStickDown; 
                    case "leftStickPress": return leftStickPress;

                    // Right Stick                    
                    case "rightStick/left": return rightStickLeft;
                    case "rightStick/right": return rightStickRight;
                    case "rightStick/up": return rightStickUp;
                    case "rightStick/down": return rightStickDown;
                    case "rightStickPress": return rightStickPress;
                }
                return null;
            }
        }
    }
}
