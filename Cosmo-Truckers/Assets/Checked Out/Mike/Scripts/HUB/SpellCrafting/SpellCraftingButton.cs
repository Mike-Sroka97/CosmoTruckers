using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellCraftingButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] bool spec = true;
    [SerializeField] int id;

    SpellCraftingController controller;

    public void OnSelect(BaseEventData eventData)
    {
        if (!controller)
            controller = MathHelpers.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.SetYapAura(id, spec);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!controller)
            controller = MathHelpers.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.ResetYapAura();
    }
}
