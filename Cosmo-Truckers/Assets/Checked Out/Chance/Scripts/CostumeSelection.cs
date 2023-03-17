using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CostumeSelection : MonoBehaviour
{
    [SerializeField] Button NextPanel;
    [SerializeField] Button PrevPanel;
    [Space(5)]
    [SerializeField] Image CharacterImage;
    int Selection = 0;
    List<Sprite> Options;

    public void SetOptions(List<Sprite> options)
    {
        NextPanel.onClick.AddListener(delegate { GoToNextPanel(); });
        PrevPanel.onClick.AddListener(delegate { GoToPrevPanel(); });

        Options = new List<Sprite>(options);
        CharacterImage.color = Color.white;
        SetDisplay();
    }


    void SetDisplay()
    {
        CharacterImage.sprite = Options[Selection];
        FindObjectOfType<ChangingRoom>().UpdatePlayerSprite(Selection);
    }

    private void GoToPrevPanel()
    {
        if (Selection > 0)
            Selection--;
        else
            Selection = Options.Count - 1;

        SetDisplay();
    }

    private void GoToNextPanel()
    {
        if (Selection < Options.Count - 1)
            Selection++;
        else
            Selection = 0;

        SetDisplay();
    }



    private void OnDisable()
    {
        NextPanel.onClick.RemoveAllListeners();
        PrevPanel.onClick.RemoveAllListeners();

        CharacterImage.color = new Color(0, 0, 0, 0);
    }
}
