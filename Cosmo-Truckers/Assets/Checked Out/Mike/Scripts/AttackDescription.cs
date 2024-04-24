using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AttackDescription : MonoBehaviour
{
    public GameObject Static;
    public GameObject Screen;
    public GameObject CostSection;
    public VideoPlayer MyVideoPlayer;
    public TextMeshProUGUI MyAttackName;
    public TextMeshProUGUI MyAttackDescription;
    public TextMeshProUGUI MyCostTitle;
    public TextMeshProUGUI MyCostDescription;
    public TextMeshProUGUI MyTargetType; 

    private void OnEnable()
    {
        Static.SetActive(false);
        MyVideoPlayer.targetTexture.Release();
        Screen.SetActive(true);
        MyVideoPlayer.targetTexture.Create();
    }

    public void UpdateCost(string costTitle, string costText)
    {
        if (costText == string.Empty)
            CostSection.SetActive(false);
        else
        {
            CostSection.SetActive(true);
            MyCostDescription.text = costText;

            if (costTitle == string.Empty)
                MyCostTitle.text = "COST"; 
            else
                MyCostTitle.text = costTitle.ToUpper();
        }
    }

    public void UpdateTargetType(string targetingType)
    {
        targetingType = targetingType.Replace('_', ' '); 
        MyTargetType.text = targetingType;
    }
}
