using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionVoteButton : MonoBehaviour
{
    [SerializeField] string dimensionName;
    [SerializeField] DimensionVoteController voteController;
    [SerializeField] string sceneToLoad;

    public void OpenConfirmationMenu()
    {
        voteController.OpenConfirmationMenu(dimensionName, sceneToLoad);
    }
}
