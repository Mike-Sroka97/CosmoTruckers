using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoise : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeOutSpeed;
    [SerializeField] SpriteRenderer outlineRenderer;
    [SerializeField] Material outlineAttackMaterial;
    [SerializeField] float outlineMaxOffset = 0.3f; 

    SpriteRenderer myRenderer, myFillRenderer;
    Color startingColor;
    Material outlineStartMaterial;
    Collider2D myCollider;
    WhiteNoise minigame;
    bool active = false;

    private void Start()
    {
        myRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        myFillRenderer = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<WhiteNoise>();

        startingColor = myRenderer.color;
        outlineStartMaterial = outlineRenderer.material;
    }

    public void NewSpawn()
    {
        StopAllCoroutines();
        myRenderer.color = startingColor;
        myFillRenderer.color = myRenderer.color;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(myRenderer.color.a < 1)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, myRenderer.color.a + Time.deltaTime * fadeSpeed);
            myFillRenderer.color = myRenderer.color;
            outlineRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, 
                myRenderer.color.b, (myRenderer.color.a + Time.deltaTime * fadeSpeed) - outlineMaxOffset);

            yield return new WaitForSeconds(Time.deltaTime);

            if (myRenderer.color.a > .3f && !active)
            {
                active = true;
                myCollider.enabled = true;
                outlineRenderer.material = outlineAttackMaterial;

            }
        }
        while (myRenderer.color.a > 0)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, myRenderer.color.a - Time.deltaTime * fadeOutSpeed);
            myFillRenderer.color = myRenderer.color;
            outlineRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, 
                myRenderer.color.b, (myRenderer.color.a + Time.deltaTime * fadeSpeed) - outlineMaxOffset);

            yield return new WaitForSeconds(Time.deltaTime);

            if (myRenderer.color.a < .3f)
            {
                myCollider.enabled = false;
                outlineRenderer.material = outlineStartMaterial;
            }
        }
        active = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            minigame.Score++;
            myCollider.enabled = false;
            Debug.Log(minigame.Score);
        }
    }
}
