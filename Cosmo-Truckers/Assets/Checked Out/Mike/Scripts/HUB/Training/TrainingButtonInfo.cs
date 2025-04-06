using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingButtonInfo : MonoBehaviour
{
    public bool CharacterButton;
    public int MinigameID; //Players are 1000+ Enemies from dungeons start are 0+, summons and miscellaneous are 0-
    public int EnemyID;
    public string CharacterName;

    private void OnEnable()
    {
        if (!transform.Find("Mask/GameObject"))
            return;

        if(DetermineLockedState())
            transform.Find("Mask/GameObject").GetComponent<Image>().color = Color.black;
        else
            transform.Find("Mask/GameObject").GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// Determines if this enemy has been encountered before
    /// </summary>
    /// <returns></returns>
    public bool DetermineLockedState()
    {
        DataLogData dataLogData = SaveManager.LoadDataLogData();
        return CharacterName != "" && !dataLogData.DataFiles[CharacterName];
    }

    public void LockIcon()
    {
        if (DetermineLockedState())
            transform.Find("Mask/GameObject").GetComponent<Image>().color = Color.black;
        else
            transform.Find("Mask/GameObject").GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// Will access enemy minigames if possible
    /// </summary>
    public void AttemptButtonSelect()
    {
        if (!DetermineLockedState())
            HelperFunctions.FindNearestParentOfType<InaPractice>(transform).SetMinigameEnemySelectScreen();
    }

    /// <summary>
    /// Will access enemy minigames if possible
    /// </summary>
    public void AttemptButtonSelectMinigameOther(int otherId)
    {
        if (!DetermineLockedState())
            HelperFunctions.FindNearestParentOfType<InaPractice>(transform).SetMinigameOtherSelectScreen(2);
    }
}
