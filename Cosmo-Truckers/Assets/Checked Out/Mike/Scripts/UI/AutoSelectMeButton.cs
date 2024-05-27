using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSelectMeButton : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().Select();
    }
}
