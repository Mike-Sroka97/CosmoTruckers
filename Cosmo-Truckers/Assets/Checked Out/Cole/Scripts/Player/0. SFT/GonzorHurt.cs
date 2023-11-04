using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GonzorHurt : MonoBehaviour
{
    [SerializeField] Color hurtColor;
    [SerializeField] float hurtTime = 0.25f;
    [SerializeField] SpriteRenderer riskERenderer; 

    ObjectShaker myShaker; 
    SpriteRenderer myRenderer;
    Color defaultColor;
    bool hurt = false; 

    void Start()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        myShaker = GetComponentInChildren<ObjectShaker>();
        defaultColor = myRenderer.color;
    }

    public void Hurt()
    {
        if (!hurt)
        {
            StartCoroutine(HurtGonzor());
        }
    }

    public IEnumerator HurtGonzor()
    {
        hurt = true; 
        myRenderer.color = hurtColor;
        riskERenderer.color = hurtColor; 
        myShaker.SetShakeState(true); 
        yield return new WaitForSeconds(hurtTime);
        myRenderer.color = defaultColor;
        riskERenderer.color = defaultColor; 
        myShaker.SetShakeState(false);
        hurt = false; 
    }
}
