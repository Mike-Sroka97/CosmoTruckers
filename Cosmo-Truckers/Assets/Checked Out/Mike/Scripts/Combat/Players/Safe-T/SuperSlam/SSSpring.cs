using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSSpring : MonoBehaviour
{
    [SerializeField] SSSpring otherObject;
    [SerializeField] bool isButton;
    [SerializeField] Material buttonToggledMaterial;
    [SerializeField] Color disabledColor;
    [SerializeField] float buttonUpForce = 4f;


    Rigidbody2D playerBody;
    SpriteRenderer springSpriteRenderer;
    Material buttonStartingMaterial;
    Color startColor; 
    bool springToggled = false;

    private void Start()
    {
        if (isButton)
        {
            SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
            buttonStartingMaterial = myRenderer.material;
            startColor = myRenderer.color;
        }
        else
        {
            int childCount = gameObject.transform.parent.childCount;

            for (int i = 0; i < childCount; i++)
            {
                SpriteRenderer childRenderer = gameObject.transform.parent.GetChild(i).GetComponent<SpriteRenderer>();

                if (childRenderer != null)
                {
                    springSpriteRenderer= childRenderer;
                    break; 
                }
            }

            startColor = springSpriteRenderer.color;
            springSpriteRenderer.color = disabledColor;

            SpringToggleState(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!playerBody)
            playerBody = FindObjectOfType<SafeTINA>().GetComponent<Rigidbody2D>();

        if (isButton)
        {
            if (collision.transform.tag == "Player" && !springToggled)
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    float dotProduct = Vector3.Dot(contact.normal, Vector3.down);

                    if (dotProduct > 0.5f)
                    {
                        playerBody.AddForce(new Vector2(0, buttonUpForce), ForceMode2D.Impulse);
                        ButtonToggleState(true);
                        otherObject.SpringToggleState(true);
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isButton)
        {
            if (collision.tag == "Player")
            {
                SpringToggleState(false);
                otherObject.ButtonToggleState(false);
            }
        }
        
        if (isButton)
        {
            if (collision.tag == "PlayerAttack" && !springToggled)
            {
                ButtonToggleState(true);
                otherObject.SpringToggleState(true);
            }
        }
    }

    public void ButtonToggleState(bool toggled)
    {
        springToggled = toggled;
        GetComponent<Collider2D>().enabled = !toggled;
        SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();

        if (toggled)
        {
            myRenderer.material = buttonToggledMaterial;
            myRenderer.color = disabledColor; 
        }
        else
        {
            myRenderer.material = buttonStartingMaterial;
            myRenderer.color = startColor;
        }
    }

    public void SpringToggleState(bool toggled)
    {
        int childCount = gameObject.transform.parent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Collider2D collider = gameObject.transform.parent.GetChild(i).GetComponent<Collider2D>();

            if (collider != null)
            {
                collider.GetComponent<Collider2D>().enabled = toggled;
            }
        }

        if (toggled)
        {
            springSpriteRenderer.color = startColor;
        }
        else
        {
            springSpriteRenderer.color = disabledColor;
        }
    }
}
