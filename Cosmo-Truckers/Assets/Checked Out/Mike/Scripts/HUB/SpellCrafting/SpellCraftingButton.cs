using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellCraftingButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool Spec = true;
    public Rarity RarityType;
    [SerializeField] int id;
    [SerializeField] Material shakingMaterial;

    SpellCraftingController controller;
    bool unlocked;
    bool shaking;
    Image lockImage;
    Material defaultMaterial;
    Image selectedImage;

    public enum Rarity
    {
        Common,
        Rare,
        Legendary,
    }

    private void Awake()
    {
        if(!Spec)
        {
            lockImage = transform.Find("LockedImage").GetComponent<Image>();
            defaultMaterial = lockImage.material;
        }
    }

    /// <summary>
    /// Set desc text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.SetYapAura(id, Spec);
    }

    /// <summary>
    /// clear desc text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);

        controller.ResetYapAura();
        LockShake(false);
    }

    /// <summary>
    /// Sets the lock visual and unlocked status
    /// </summary>
    public void CheckUnlockedStatus()
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);
        if(!lockImage)
            lockImage = transform.Find("LockedImage").GetComponent<Image>();

        if (controller.PlayerData.UnlockedSpells[new System.Tuple<int, int>(controller.CurrentCharacterId, id)])
        {
            lockImage.gameObject.SetActive(false);
            unlocked = true;
        }
        else
        {
            lockImage.gameObject.SetActive(true);
            unlocked = false;
        }
    }

    /// <summary>
    /// Sets the selected visual on spec buttons
    /// </summary>
    public void CheckSelectedStatus()
    {
        if (!controller)
            controller = HelperFunctions.FindNearestParentOfType<SpellCraftingController>(transform);
        if (!selectedImage)
            selectedImage = transform.Find("SelectedGO").GetComponent<Image>();

        if (controller.PlayerData.SelectedSpecs[controller.CurrentCharacterId] == id)
            selectedImage.enabled = true;
        else
            selectedImage.enabled = false;
    }

    /// <summary>
    /// Selects and saves a spec
    /// </summary>
    public void AttemptSpecSelect()
    {
        if (controller.PlayerData.SelectedSpecs[controller.CurrentCharacterId] == id)
            return;

        controller.PlayerData = controller.PlayerData.SaveSpecSelection(controller.CurrentCharacterId, id);
        controller.ResetSpec();
    }

    /// <summary>
    /// Start unlock or unlock spell with valid token
    /// </summary>
    public void AttemptUnlock()
    {
        if (unlocked || !TokenValidtyCheck())
            return;

        if(!shaking)
        {
            controller.ResetLockShake();
            LockShake(true);
        }
        else
        {
            unlocked = true;
            controller.PlayerData.SaveSpellUnlock(new System.Tuple<int, int>(controller.CurrentCharacterId, id), true, RarityType);
            controller.SetTokenText();
            controller.ResetLockShake();
            lockImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Start/Stop lock shake
    /// </summary>
    public void LockShake(bool shake)
    {
        if (!lockImage)
            return;

        shaking = shake;

        if (shake)
        {
            lockImage.material = shakingMaterial;
            lockImage.GetComponent<ObjectShaker>().SetShakeState(true);
        }
        else
        {
            lockImage.material = defaultMaterial;
            lockImage.GetComponent<ObjectShaker>().SetShakeState(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool TokenValidtyCheck()
    {
        switch (RarityType)
        {
            case Rarity.Common:
                if (controller.PlayerData.CommonSpellTokens > 0)
                    return true;
                else
                    return false;
            case Rarity.Rare:
                if (controller.PlayerData.RareSpellTokens > 0)
                    return true;
                else
                    return false;
            case Rarity.Legendary:
                if (controller.PlayerData.LegendarySpellTokens > 0)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }
}
