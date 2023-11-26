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
    [SerializeField] TMP_Text AUGDescription;
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

        charactersAUGS = currentChar.GetAUGS;

        for(int i = 0; i < charactersAUGS.Count; i++)
        {
            selectable[i].GetComponentInChildren<TMP_Text>().text = charactersAUGS[i].CurrentStacks.ToString();
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

        if(currentLocation - 4 < 0)
        {
            newLocation = currentLocation + (selectable.Count - 4);
        }
        else
        {
            newLocation = currentLocation - 4;
        }

        ShowSelection(newLocation);
    }
    void MoveDown()
    {
        int newLocation = 0;

        if (currentLocation + 4 >= selectable.Count)
        {
            newLocation = currentLocation - (selectable.Count - 4);
        }
        else
        {
            newLocation = currentLocation + 4;
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

        currentLocation = newLocation;
        SetDescription();
    }
}
