using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    [SerializeField] protected Material activeMaterial;
    [SerializeField] protected Material toggledMaterial;
    public virtual bool CanBeToggled
    { 
        get
        {
            return canBeToggled;
        }
        protected set
        {
            if(value == true)
            {
                SetToggle(activeMaterial, true); 
            }
            else
            {
                SetToggle(toggledMaterial, false);
            }
        }
    }

    [SerializeField] private bool canBeToggled = true;
    protected SpriteRenderer myRenderer;

    private void Start()
    {
        Initialize(); 
    }

    protected virtual void Initialize()
    {
        myRenderer = GetComponent<SpriteRenderer>();

        if (canBeToggled)
            myRenderer.material = activeMaterial;
        else
            myRenderer.material = toggledMaterial;
    }

    protected abstract void ToggleMe();

    private void SetToggle(Material material, bool toggled)
    {
        myRenderer.material = material;
        canBeToggled = toggled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack") && CanBeToggled)
        {
            CanBeToggled = false;
            ToggleMe();
        }
    }
}
