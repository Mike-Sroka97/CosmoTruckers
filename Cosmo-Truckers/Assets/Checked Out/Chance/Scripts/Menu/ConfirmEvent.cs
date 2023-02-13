using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmEvent : MonoBehaviour
{
    [SerializeField] TMP_Text DisplayText;
    [SerializeField] Button Yes;
    [SerializeField] Button No;

    public delegate void Function();


    public void ChangeDisplayText(string text) => DisplayText.text = text;

    public void AddListner(bool value, Function f)
    {
        if (value)
            Yes.onClick.AddListener(delegate { f(); });
        else
            No.onClick.AddListener(delegate { f(); });
    }

    public void RemoveListners()
    {
        Yes.onClick.RemoveAllListeners();
        No.onClick.RemoveAllListeners();
    }
}
