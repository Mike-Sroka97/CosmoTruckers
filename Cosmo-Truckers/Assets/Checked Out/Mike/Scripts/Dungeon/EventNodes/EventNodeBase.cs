using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventNodeBase : MonoBehaviour
{
    [SerializeField] protected float delay = 2.6f;
    [SerializeField] protected AugmentStackSO[] augmentsToAdd;
    [HideInInspector] public EventNodePopupInfo PopupOne;
    [HideInInspector] public EventNodePopupInfo PopupTwo;

    protected Button[] myButtons;
    protected EventNodeHandler nodeHandler;
    protected TextMeshProUGUI descriptionText;
    protected int currentTurns = 0;
    protected string characterNameColor = EnumManager.ColorsHex[EnumManager.ColorPalette.Purple];


    protected PlayerCharacter currentCharacter => PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter; 

    protected virtual void Start()
    {
        descriptionText = transform.Find("Description/Description Text").GetComponent<TextMeshProUGUI>();
        myButtons = GetComponentsInChildren<Button>();
        PopupOne = transform.Find("Popups/Event Node Info (1)").GetComponent<EventNodePopupInfo>();
        PopupTwo = transform.Find("Popups/Event Node Info (2)").GetComponent<EventNodePopupInfo>();
        HandleButtonDeselect();
    }

    public virtual void Initialize(EventNodeHandler handler)
    {
        nodeHandler = handler;
        GetComponentInChildren<Button>().Select();
    }

    protected virtual IEnumerator SelectionChosen()
    {
        foreach (Button button in myButtons)
        {
            button.enabled = false;
            button.GetComponent<EventNodeButton>().DeleteMaterial();
        }
        
        yield return new WaitForSeconds(delay);
        StartCoroutine(nodeHandler.Move(false));
    }

    protected void IteratePlayerReference()
    {
        nodeHandler.Player++;
        if (nodeHandler.Player > 3)
            nodeHandler.Player = 0;
    }

    protected virtual void CheckEndEvent()
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
        AutoSelectNextButton();
    }

    public virtual void IgnoreOption()
    {
        IteratePlayerReference();
        AutoSelectNextButton();
        CheckEndEvent();
    }

    public void IgnoreAndCheckDisable(int buttonToDisable = 1)
    {
        IgnoreOption();

        if (currentTurns == 3)
            myButtons[buttonToDisable].enabled = false;
    }

    protected void AutoSelectNextButton()
    {
        //Selects next available button
        foreach (Button button in myButtons)
            if (button.enabled)
            {
                if (FindObjectOfType<EventSystem>().currentSelectedGameObject == button.gameObject)
                    button.GetComponent<EventNodeButton>().ResetMaterial();
                else
                    button.Select();
                break;
            }
    }

    protected void AddAugmentToPlayer(AugmentStackSO augment, int amount = 1)
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.AddAugmentStack(augment, amount);
    }

    public virtual void HandleButtonSelect(int buttonId)
    {

    }

    public void HandleButtonDeselect()
    {
        PopupOne.gameObject.SetActive(false);
        PopupTwo.gameObject.SetActive(false);
    }
}
