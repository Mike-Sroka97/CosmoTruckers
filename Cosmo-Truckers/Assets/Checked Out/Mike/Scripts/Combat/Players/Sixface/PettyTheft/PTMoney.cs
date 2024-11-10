using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTMoney : MonoBehaviour
{
    [SerializeField] GameObject particle; 
    PettyTheft minigame;
    SpriteRenderer myRenderer;

    private void Start()
    {
        minigame = FindObjectOfType<PettyTheft>();
        myRenderer = GetComponent<SpriteRenderer>();

        //Set outline for money to off 
        myRenderer.material.SetInt("_MainValue", 0); 
    }

    public void Activate()
    {
        GetComponent<Collider2D>().enabled = true;
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1);

        //Set outline for money to on 
        myRenderer.material.SetInt("_MainValue", 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.Score++;
            minigame.CheckSuccess(); 
            Debug.Log(minigame.Score);
            Instantiate(particle, transform.position, Quaternion.identity, minigame.transform);
            Destroy(transform.parent.gameObject);
        }
    }
}
