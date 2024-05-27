using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionVoteButton : MonoBehaviour
{
    [SerializeField] int voteValue;
    [SerializeField] string dimensionName;
    [SerializeField] DimensionVoteController voteController;

    public void OpenConfirmationMenu()
    {
        voteController.OpenConfirmationMenu(voteValue, dimensionName);
    }
}
