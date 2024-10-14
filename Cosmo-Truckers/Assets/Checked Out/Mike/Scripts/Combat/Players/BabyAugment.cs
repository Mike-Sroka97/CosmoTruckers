using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabyAugment : MonoBehaviour
{
    [SerializeField] Sprite[] bgs;

    Image bg;
    Image augImage;

    public void UpdateSlot(AugmentStackSO aug)
    {
        gameObject.SetActive(true);
        SetBG(aug);
        augImage.sprite = aug.AugmentSprite;
    }

    private void SetBG(AugmentStackSO aug)
    {
        if(!bg)
        {
            bg = GetComponent<Image>();
            augImage = transform.Find("Image").GetComponent<Image>();
        }

        if (!aug.IsBuff && !aug.IsDebuff && aug.Removable)
            bg.sprite = bgs[0];

        else if (!aug.IsBuff && !aug.IsDebuff && !aug.Removable)
            bg.sprite = bgs[1];

        else if (aug.IsBuff && aug.Removable)
            bg.sprite = bgs[2];

        else if (aug.IsBuff && !aug.Removable)
            bg.sprite = bgs[3];

        else if (aug.IsDebuff && aug.Removable)
            bg.sprite = bgs[4];

        else if (aug.IsDebuff && !aug.Removable)
            bg.sprite = bgs[5];
    }
}
