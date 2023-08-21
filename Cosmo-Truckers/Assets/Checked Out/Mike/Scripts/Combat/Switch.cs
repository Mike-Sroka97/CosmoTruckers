using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    [SerializeField] Material activeMaterial;
    [SerializeField] Material toggledMaterial;
    public bool CanBeToggled
    { 
        get
        {
            return canBeToggled;
        }
        set
        {
            if(value == true)
            {
                myRenderer.material = activeMaterial;
                canBeToggled = true;
            }
            else
            {
                myRenderer.material = toggledMaterial;
                canBeToggled = false;
            }
        }
    }

    private bool canBeToggled = true;
    protected SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    protected abstract void ToggleMe();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && CanBeToggled)
        {
            CanBeToggled = false;
            ToggleMe();
        }
    }
}
