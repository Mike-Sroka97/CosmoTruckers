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
    public VideoPlayer MyVideoPlayer;
    public TextMeshProUGUI MyAttackName;
    public TextMeshProUGUI MyAttackDescription;

    private void OnEnable()
    {
        Static.SetActive(false);
        Screen.SetActive(true);
    }
}
