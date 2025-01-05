using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermassiveAmplifierBlackHole : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial; 
    [SerializeField] private float disabledOpacity = 0.4f; 
    CircleCollider2D myBlackHole; //smirking emoji
    Graviton myGraviton;
    SpriteRenderer myRenderer;
    SpriteRenderer myChildRenderer;
    Material enabledMaterial; 
    Color disabledColor;
    Color enabledColor;

    public bool ActiveState { get; private set; } = true; 

    private void Awake()
    {
        myBlackHole = GetComponent<CircleCollider2D>();
        myGraviton = GetComponent<Graviton>();
        myRenderer = GetComponent<SpriteRenderer>();
        myChildRenderer = GetComponentInChildren<SpriteRenderer>();

        enabledColor = myRenderer.color;
        enabledMaterial = myRenderer.material;
        disabledColor = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, disabledOpacity);
    }

    public void SetActiveState(bool state)
    {
        ActiveState = state;

        myBlackHole.enabled = state;
        myGraviton.enabled = state;  

        if (state) 
        { 
            myRenderer.color = enabledColor;
            myChildRenderer.color = enabledColor; 
            myRenderer.material = enabledMaterial;
            myChildRenderer.material = enabledMaterial;
        }
        else 
        { 
            myRenderer.color = disabledColor;
            myChildRenderer.color = disabledColor;
            myRenderer.material = disabledMaterial;
            myChildRenderer.material = disabledMaterial;
        }
    }
}
