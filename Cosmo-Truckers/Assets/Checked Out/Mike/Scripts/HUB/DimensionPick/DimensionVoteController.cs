using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DimensionVoteController : MonoBehaviour
{
    [SerializeField] GameObject confirmationScreen;
    [SerializeField] GameObject voteScreen;

    const string confirmationString = "Are you sure you want to enter the ";
    string currentDimension;

    public void OpenConfirmationMenu(string dimensionName, string sceneToLoad)
    {
        confirmationScreen.SetActive(true);
        confirmationScreen.transform.Find("ConfirmationText").GetComponent<TextMeshProUGUI>().text = confirmationString + dimensionName + "?";
        currentDimension = sceneToLoad;
        voteScreen.SetActive(false);
    }
    public void LoadMyDimension()
    {
        StartCoroutine(CameraController.Instance.HubSceneLoad(currentDimension));
    }

    public void CloseConfirmationMenu()
    {
        voteScreen.SetActive(true);
        confirmationScreen.SetActive(false);
    }
}
