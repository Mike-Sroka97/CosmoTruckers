using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataLogController : MonoBehaviour
{
    [Header("Main Screen")]
    [SerializeField] GameObject mainScreen;

    [Space(20)]
    [Header("Secondary Screens")]
    [SerializeField] GameObject secondaryScreen;
    [SerializeField] GameObject[] secondaryScreens;

    [Space(20)]
    [Header("Yap")]
    public TextMeshProUGUI DataYapAura;

    /// <summary>
    /// On enable sets default state of the data log
    /// </summary>
    private void OnEnable()
    {
        ResetMe();
    }

    /// <summary>
    /// Sets default state of the data log
    /// </summary>
    public void ResetMe()
    {
        mainScreen.SetActive(true);
        secondaryScreen.SetActive(false);
        DataYapAura.text = "";
    }

    /// <summary>
    /// Primes secondary screens
    /// </summary>
    private void SetSecondaryScreens()
    {
        mainScreen.SetActive(false);
        foreach (GameObject go in secondaryScreens)
            go.SetActive(false);
        secondaryScreen.SetActive(true);
        DataYapAura.text = "";
    }

    public void SetSecondaryScreen(GameObject screenToSet)
    {
        SetSecondaryScreens();
        screenToSet.SetActive(true);
    }
}
