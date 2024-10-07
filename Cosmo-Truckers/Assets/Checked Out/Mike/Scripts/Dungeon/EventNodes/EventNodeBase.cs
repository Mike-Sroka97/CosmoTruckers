using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventNodeBase : MonoBehaviour
{
    [SerializeField] protected float delay = 2.6f;
    [SerializeField] protected DebuffStackSO[] augmentsToAdd;

    protected EventNodeHandler nodeHandler;
    protected TextMeshProUGUI descriptionText;

    private void Start()
    {
        descriptionText = transform.Find("Description/Description Text").GetComponent<TextMeshProUGUI>();
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
}
