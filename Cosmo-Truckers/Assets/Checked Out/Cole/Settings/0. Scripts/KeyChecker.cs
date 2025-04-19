using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyChecker : MonoBehaviour
{
    [HideInInspector]
    public Key[] allKeys = new Key[]
    {
    Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
    Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
    Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
    Key.V, Key.W, Key.X, Key.Y, Key.Z,

    Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
    Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9,

    Key.F1, Key.F2, Key.F3, Key.F4, Key.F5,
    Key.F6, Key.F7, Key.F8, Key.F9, Key.F10,
    Key.F11, Key.F12,

    Key.Backquote, Key.Minus, Key.Equals, Key.LeftBracket, Key.RightBracket,
    Key.Backslash, Key.Semicolon, Key.Quote, Key.Comma, Key.Period, Key.Slash,

    Key.Space, Key.Enter, Key.Tab, Key.Backspace, Key.Escape,

    Key.LeftArrow, Key.RightArrow, Key.UpArrow, Key.DownArrow,

    Key.LeftShift, Key.RightShift,
    Key.LeftCtrl, Key.RightCtrl,
    Key.LeftAlt, Key.RightAlt,
    Key.LeftMeta, Key.RightMeta,
    Key.CapsLock, Key.NumLock, Key.ScrollLock,

    Key.PrintScreen, Key.Pause, Key.Insert, Key.Delete,
    Key.Home, Key.End, Key.PageUp, Key.PageDown,

    Key.NumpadEnter, Key.NumpadEquals,
    Key.NumpadDivide, Key.NumpadMultiply, Key.NumpadMinus,
    Key.NumpadPlus, Key.NumpadPeriod,
    Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3,
    Key.Numpad4, Key.Numpad5, Key.Numpad6, Key.Numpad7,
    Key.Numpad8, Key.Numpad9,
    };
}
