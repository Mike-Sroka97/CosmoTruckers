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
    [SerializeField] GameObject[] npcScreens;
    [SerializeField] GameObject[] enemiesScreens;
    [SerializeField] GameObject[] enemySummonsScreens;
    [SerializeField] GameObject[] friendlySummonsScreens;

    [Space(20)]
    [Header("Yap")]
    public TextMeshProUGUI DataYapAura;

    [HideInInspector] public int currentDimension;

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

    /// <summary>
    /// Sets a derivative screen of the main screen
    /// </summary>
    /// <param name="screenToSet"></param>
    public void SetSecondaryScreen(GameObject screenToSet)
    {
        SetSecondaryScreens();
        screenToSet.SetActive(true);
    }

    /// <summary>
    /// Sets single dimension data screen to be DRY
    /// </summary>
    /// <param name="screenToSet"></param>
    /// <param name="dimension"></param>
    public void SetDungeonScreen(int dimension)
    {
        currentDimension = dimension;
    }

    public void SetNpcScreen()
    {
        SetSecondaryScreen(npcScreens[currentDimension - 1]);
    }

    public void SetEnemyScreen()
    {
        SetSecondaryScreen(enemiesScreens[currentDimension - 1]);
    }

    public void SetEnemySummonScreen()
    {
        SetSecondaryScreen(enemySummonsScreens[currentDimension - 1]);
    }

    public void SetFriendlySummonScreen()
    {
        SetSecondaryScreen(friendlySummonsScreens[currentDimension - 1]);
    }
}
