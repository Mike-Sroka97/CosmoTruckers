using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventNodeBase : MonoBehaviour
{
    [SerializeField] protected float delay = 2.6f;
    [SerializeField] protected DebuffStackSO[] augmentsToAdd;

    protected Button[] myButtons;
    protected EventNodeHandler nodeHandler;
    protected TextMeshProUGUI descriptionText;
    protected int currentTurns = 0;

    protected virtual void Start()
    {
        descriptionText = transform.Find("Description/Description Text").GetComponent<TextMeshProUGUI>();
        myButtons = GetComponentsInChildren<Button>();
    }

    public virtual void Initialize(EventNodeHandler handler)
    {
        nodeHandler = handler;
        GetComponentInChildren<Button>().Select();
    }

    protected virtual IEnumerator SelectionChosen()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
            button.enabled = false;
        
        yield return new WaitForSeconds(delay);
        StartCoroutine(nodeHandler.Move(false));
    }

    protected void IteratePlayerReference()
    {
        nodeHandler.Player++;
        if (nodeHandler.Player > 3)
            nodeHandler.Player = 0;
    }

    protected void CheckEndEvent()
    {
        currentTurns++;

        if (currentTurns > 3)
            StartCoroutine(SelectionChosen());
    }

    protected void MultiplayerSelection(int buttonID)
    {
        //Disable button and iterate player
        myButtons[buttonID].GetComponent<EventNodeButton>().MultiplayerSelected = true;
        myButtons[buttonID].enabled = false;
        IteratePlayerReference();

        //Selects next available button
        foreach (Button button in myButtons)
            if (button.enabled)
            {
                button.Select();
                break;
            }
    }
}
