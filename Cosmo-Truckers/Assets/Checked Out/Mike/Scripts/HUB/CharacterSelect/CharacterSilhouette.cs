using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSilhouette : MonoBehaviour
{
    [SerializeField] int index;
    [HideInInspector] public bool Jaunting;

    SpriteRenderer myRenderer;

    public void SetSprite(Sprite characterSprite)
    {
        if (!myRenderer)
            myRenderer = GetComponentInChildren<SpriteRenderer>();

        myRenderer.sprite = characterSprite;
    }

    public void ResetAll()
    {
        transform.parent.GetComponentInParent<HUBController>().SetSilhouette(index);
    }

    public void StartJaunt()
    {
        Jaunting = true;
    }

    public void EndJaunt()
    {
        Jaunting = false;
    }
}
