using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDIALOG : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DialogManager.instance.EnterDialogMode(inkJSON);
        }
    }
}
