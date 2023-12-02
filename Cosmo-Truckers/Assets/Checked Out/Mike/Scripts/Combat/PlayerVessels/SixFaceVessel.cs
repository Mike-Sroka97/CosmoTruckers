using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SixFaceVessel : PlayerVessel
{
    [SerializeField] GameObject smugFace;
    [SerializeField] GameObject hypeFace;
    [SerializeField] GameObject sadFace;
    [SerializeField] GameObject moneyFace;
    [SerializeField] GameObject dizzyFace;
    [SerializeField] GameObject megalomanicFace;

    Color activeColor = Color.black;
    Color deactiveColor = new Color (1, 1, 1, .6f);
    Color fullColor = Color.white;

    public void UpdateFace(SixFaceMana.FaceTypes faceType)
    {
        ClearFaces();

        //TODO mego track here

        Image[] faceRenderers;

        switch (faceType)
        {
            case SixFaceMana.FaceTypes.Smug:
                faceRenderers = smugFace.GetComponentsInChildren<Image>();
                faceRenderers[0].color = activeColor;
                faceRenderers[1].color = fullColor;
                break;
            case SixFaceMana.FaceTypes.Hype:
                faceRenderers = hypeFace.GetComponentsInChildren<Image>();
                faceRenderers[0].color = activeColor;
                faceRenderers[1].color = fullColor;
                break;
            case SixFaceMana.FaceTypes.Sad:
                faceRenderers = sadFace.GetComponentsInChildren<Image>();
                faceRenderers[0].color = activeColor;
                faceRenderers[1].color = fullColor;
                break;
            case SixFaceMana.FaceTypes.Money:
                faceRenderers = moneyFace.GetComponentsInChildren<Image>();
                faceRenderers[0].color = activeColor;
                faceRenderers[1].color = fullColor;
                break;
            case SixFaceMana.FaceTypes.Dizzy:
                faceRenderers = dizzyFace.GetComponentsInChildren<Image>();
                faceRenderers[0].color = activeColor;
                faceRenderers[1].color = fullColor;
                break;
            default:
                break;
        }
    }

    private void ClearFaces()
    {
        Image[] faceRenderers;

        //lol
        faceRenderers = smugFace.GetComponentsInChildren<Image>();
        faceRenderers[1].color = deactiveColor;
        faceRenderers = hypeFace.GetComponentsInChildren<Image>();
        faceRenderers[1].color = deactiveColor;
        faceRenderers = sadFace.GetComponentsInChildren<Image>();
        faceRenderers[1].color = deactiveColor;
        faceRenderers = moneyFace.GetComponentsInChildren<Image>();
        faceRenderers[1].color = deactiveColor;
        faceRenderers = dizzyFace.GetComponentsInChildren<Image>();
        faceRenderers[1].color = deactiveColor;
    }

    public void ClearFaceBG()
    {
        Image[] faceRenderers;

        //lol again
        faceRenderers = smugFace.GetComponentsInChildren<Image>();
        faceRenderers[0].color = deactiveColor;
        faceRenderers = hypeFace.GetComponentsInChildren<Image>();
        faceRenderers[0].color = deactiveColor;
        faceRenderers = sadFace.GetComponentsInChildren<Image>();
        faceRenderers[0].color = deactiveColor;
        faceRenderers = moneyFace.GetComponentsInChildren<Image>();
        faceRenderers[0].color = deactiveColor;
        faceRenderers = dizzyFace.GetComponentsInChildren<Image>();
        faceRenderers[0].color = deactiveColor;

        megalomanicFace.SetActive(false);
    }

    public bool CheckMegalomanicMode()
    {
        bool megaManic = true;

        Image[] faceRenderers;

        faceRenderers = smugFace.GetComponentsInChildren<Image>();
        if (faceRenderers[0].color != activeColor)
            megaManic = false;
        faceRenderers = hypeFace.GetComponentsInChildren<Image>();
        if (faceRenderers[0].color != activeColor)
            megaManic = false;
        faceRenderers = sadFace.GetComponentsInChildren<Image>();
        if (faceRenderers[0].color != activeColor)
            megaManic = false;
        faceRenderers = moneyFace.GetComponentsInChildren<Image>();
        if (faceRenderers[0].color != activeColor)
            megaManic = false;
        faceRenderers = dizzyFace.GetComponentsInChildren<Image>();
        if (faceRenderers[0].color != activeColor)
            megaManic = false;

        if (megaManic)
            megalomanicFace.SetActive(true);

        return megaManic;
    }
}
