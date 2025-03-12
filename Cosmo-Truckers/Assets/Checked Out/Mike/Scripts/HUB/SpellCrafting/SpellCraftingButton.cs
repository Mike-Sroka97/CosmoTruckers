using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellCraftingButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool Spec = true;
    [SerializeField] int id;

    SpellCraftingController controller;
    bool unlocked;

    public void OnSelect(BaseEventData eventData)
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.SetYapAura(id, Spec);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.ResetYapAura();
    }

    public void CheckUnlockedStatus()
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        if (controller.PlayerData.UnlockedSpells[new System.Tuple<int, int>(controller.CurrentCharacterId, id)])
        {
            transform.Find("LockedImage").gameObject.SetActive(false);
            unlocked = true;
        }
        else
        {
            transform.Find("LockedImage").gameObject.SetActive(true);
            unlocked = false;
        }
    }
}
