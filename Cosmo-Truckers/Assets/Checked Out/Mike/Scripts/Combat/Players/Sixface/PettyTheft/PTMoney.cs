using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTMoney : MonoBehaviour
{
    PettyTheft minigame;
    SpriteRenderer myRenderer;

    private void Start()
    {
        minigame = FindObjectOfType<PettyTheft>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        GetComponent<Collider2D>().enabled = true;
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            Destroy(transform.parent.gameObject);
        }
    }
}
