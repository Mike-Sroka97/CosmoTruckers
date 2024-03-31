using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AUG_DESCRIPTION : MonoBehaviour
{
    Character currentChar;
    List<DebuffStackSO> charactersAUGS;


    [SerializeField] List<GameObject> selectable;
    [SerializeField] Color selectedColor;
    [SerializeField] Color baseColor;
    [SerializeField] Color nonFadingColor;
    [SerializeField] Color fadingColor;
    [SerializeField] TMP_Text AUGDescription;
    [SerializeField] Sprite[] bgs;
    int currentLocation = 0;

    private void OnDisable()
    {
        currentChar = null;
        charactersAUGS.Clear();

        foreach(var stack in selectable)
            stack.GetComponentInChildren<TMP_Text>().text = "";

        AUGDescription.text = "";
    }

    /// <summary>
    /// Init the AUG list to show all AUGs on character
    /// </summary>
    /// <param name="character">Base class for all characters that holds the AUGS</param>
    public void InitList(Character character)
    {
        currentChar = character;

        charactersAUGS = new(currentChar.GetAUGS);

        for(int i = 0; i < charactersAUGS.Count; i++)
        {
            selectable[i].GetComponentInChildren<TMP_Text>().text = charactersAUGS[i].CurrentStacks.ToString();

            if(charactersAUGS[i].GetFade() != 0)
                selectable[i].GetComponentsInChildren<TMP_Text>()[1].text = "-" + charactersAUGS[i].GetFade().ToString();
            else
                selectable[i].GetComponentsInChildren<TMP_Text>()[1].text = null;

            selectable[i].GetComponentsInChildren<Image>()[1].sprite = charactersAUGS[i].AugmentSprite;
        }
        for(int i = 0; i < 6; i++)
        {
            DetermineBG(i);
        }

        ShowSelection(0);
    }

    //Temp movement system
    //TODO Mike
    private void Update()
    {
        //Somehow we started this script without setting up the character
        if (currentChar == null)
        {
            Debug.LogError("InitList must be called on this obj before update is called");
            return;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            //-4
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //-1
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            //+4
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //+1
            MoveRight();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }

    void SetDescription()
    {
        string AUGName = charactersAUGS.Count > currentLocation ? charactersAUGS[currentLocation].DebuffName : "Empty";
        string AUGDes = charactersAUGS.Count > currentLocation ? charactersAUGS[currentLocation].DebuffDescription : "";

        AUGDescription.text = $"<align=center><b>{AUGName}</b></align>\n{AUGDes}";
    }

    void MoveUp()
    {
        int newLocation = 0;

        if(currentLocation - 3 < 0)
        {
            newLocation = currentLocation + (selectable.Count - 3);
        }
        else
        {
            newLocation = currentLocation - 3;
        }

        ShowSelection(newLocation);
    }
    void MoveDown()
    {
        int newLocation = 0;

        if (currentLocation + 3 >= selectable.Count)
        {
            newLocation = currentLocation - (selectable.Count - 3);
        }
        else
        {
            newLocation = currentLocation + 3;
        }

        ShowSelection(newLocation);
    }
    void MoveRight()
    {
        int newLocation = 0;

        if (currentLocation + 1 >= selectable.Count)
        {
            newLocation = 0;
        }
        else
        {
            newLocation = currentLocation + 1;
        }

        ShowSelection(newLocation);
    }
    void MoveLeft()
    {
        int newLocation = 0;

        if (currentLocation - 1 < 0)
        {
            newLocation = selectable.Count - 1;
        }
        else
        {
            newLocation = currentLocation - 1;
        }

        ShowSelection(newLocation);
    }

    void ShowSelection(int newLocation)
    {
        selectable[currentLocation].GetComponent<Image>().color = baseColor;
        selectable[newLocation].GetComponent<Image>().color = selectedColor;

        if(currentLocation + 1 <= charactersAUGS.Count)
            selectable[currentLocation].GetComponentsInChildren<Image>()[1].color = baseColor;
        if (newLocation + 1 <= charactersAUGS.Count)
            selectable[newLocation].GetComponentsInChildren<Image>()[1].color = selectedColor;

        currentLocation = newLocation;
        SetDescription();
    }

    private void DetermineBG(int currentSlot)
    {
        selectable[currentSlot].GetComponentsInChildren<Image>()[1].color = baseColor;
        selectable[currentSlot].GetComponentsInChildren<Image>()[0].color = baseColor;

        if (currentSlot + 1 > charactersAUGS.Count)
        {
            selectable[currentSlot].GetComponentInChildren<TMP_Text>().text = null;
            selectable[currentSlot].GetComponentsInChildren<TMP_Text>()[1].text = null;
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[6];
            selectable[currentSlot].GetComponentsInChildren<Image>()[1].sprite = null;
            selectable[currentSlot].GetComponentsInChildren<Image>()[1].color = Color.clear;
            return;
        }


        //Colors fun
        if (!charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].IsDebuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[0];

        else if(!charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].IsDebuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[1];

        else if (charactersAUGS[currentSlot].IsBuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[2];

        else if(charactersAUGS[currentSlot].IsBuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[3];

        else if (charactersAUGS[currentSlot].IsDebuff && charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[4];

        else if (charactersAUGS[currentSlot].IsDebuff && !charactersAUGS[currentSlot].Removable)
            selectable[currentSlot].GetComponentsInChildren<Image>()[0].sprite = bgs[5];
    }
}
